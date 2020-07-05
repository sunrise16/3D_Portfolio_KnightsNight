using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill : MonoBehaviour
{
    #region 컴포넌트 관련 변수

    // 스킬 1 발사 오브젝트
    public GameObject skill1Object;
    // 스킬 1 발사 좌표
    public GameObject skillFirePoint;
    // 스킬 쿨타임 게이지 이미지 오브젝트
    public GameObject skill1CooldownImage;
    public GameObject skill2CooldownImage;
    // 스킬 이펙트
    public GameObject skill1Effect;
    public GameObject[] skill2Effect;
    // 오디오 소스 컴포넌트
    private AudioSource audio;
    public AudioClip[] audioClip;
    // 스킬 사용 불가 메세지
    public GameObject cannotUseSkillMessage;

    #endregion

    #region 제어값 관련 변수

    // 스킬 사용 불가 메세지 알파값
    private float messageAlpha;

    #endregion

    void Start()
    {
        #region 컴포넌트 변수 관련 초기화

        // 오디오 소스 컴포넌트 할당
        audio = GetComponent<AudioSource>();

        #endregion

        #region 제어값 변수 관련 초기화

        // 스킬 사용 불가 메세지 알파값 초기화
        messageAlpha = 0.0f;

        #endregion
    }

    void Update()
    {
        #region 실시간 변수값 갱신

        // 스킬 사용 불가 메세지 알파값 갱신
        messageAlpha -= Time.deltaTime;
        if (messageAlpha <= 0.0f)
        {
            messageAlpha = 0.0f;
        }
        
        // 이미지 페이드 인, 아웃 처리
        cannotUseSkillMessage.GetComponent<Text>().color = new Color(255, 255, 255, messageAlpha);

        #endregion

        #region 키보드 스킬 사용

        if ((GetComponent<PlayerFSM>().playerState != PlayerFSM.PlayerState.Damaged &&
            GetComponent<PlayerFSM>().playerState != PlayerFSM.PlayerState.Jump &&
            GetComponent<PlayerFSM>().playerState != PlayerFSM.PlayerState.SkillUse) && Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayerSkill1Use();
        }
        if ((GetComponent<PlayerFSM>().playerState != PlayerFSM.PlayerState.Damaged &&
            GetComponent<PlayerFSM>().playerState != PlayerFSM.PlayerState.Jump &&
            GetComponent<PlayerFSM>().playerState != PlayerFSM.PlayerState.SkillUse) && Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayerSkill2Use();
        }

        #endregion
    }

    #region 플레이어 스킬 1 사용

    public void PlayerSkill1Use()
    {
        // 스킬 발동 조건을 전부 만족하고 있을 경우
        if (DataManager.instance.playerCurrentMP >= 30 && DataManager.instance.playerSkill1Cooldown <= 0)
        {
            // 마력 감소
            DataManager.instance.playerCurrentMP -= 30;

            // 쿨타임 갱신
            DataManager.instance.playerSkill1Cooldown = 8;
            DataManager.instance.skill1CooldownDelay = 8.0f;
            skill1CooldownImage.SetActive(true);

            // 스킬 1 오브젝트 생성 후 전방 발사
            GameObject fireball = Instantiate(skill1Object, skillFirePoint.transform.position, Quaternion.identity);
            fireball.transform.forward = (skillFirePoint.transform.forward).normalized;
            fireball.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
            fireball.transform.SetParent(GameObject.Find("Object Pool").transform.GetChild(1).transform);

            // 플레이어 상태 변경
            GetComponent<PlayerFSM>().PlayerSkillUse();

            // 이펙트 출력
            StartCoroutine(Skill1Effect());
        }
        // 스킬 발동 조건을 만족시키고 있지 않을 경우
        else
        {
            audio.clip = audioClip[0];
            audio.Play();
            messageAlpha = 1.0f;
        }
    }

    #endregion

    #region 플레이어 스킬 2 사용

    public void PlayerSkill2Use()
    {
        // 스킬 발동 조건을 전부 만족하고 있을 경우
        if (DataManager.instance.playerCurrentMP >= 40 && DataManager.instance.playerSkill2Cooldown <= 0)
        {
            // 마력 감소
            DataManager.instance.playerCurrentMP -= 40;

            // 쿨타임 갱신
            DataManager.instance.playerSkill2Cooldown = 10;
            DataManager.instance.skill2CooldownDelay = 10.0f;
            skill2CooldownImage.SetActive(true);

            // 스킬 발동 시 주변의 적 몬스터 오브젝트 검출 후 데미지
            Collider[] colls = Physics.OverlapSphere(transform.position, 8.0f, 1 << 10 | 1 << 11);
            foreach (var coll in colls)
            {
                if (coll.GetComponent<EnemyInfo>().enemyCurrentHP > 0)
                {
                    GetComponent<PlayerFSM>().playerComboCounting = true;
                    GetComponent<PlayerFSM>().comboDelay = 0.0f;
                    DataManager.instance.totalScore += 10 + (DataManager.instance.playerCurrentCombo);
                    DataManager.instance.playerCurrentCombo++;
                    coll.GetComponent<EnemyFSM>().HitDamage(DataManager.instance.playerSkill2Damage);
                }
            }

            // 플레이어 상태 변경
            GetComponent<PlayerFSM>().PlayerSkillUse();

            // 이펙트 출력
            StartCoroutine(Skill2Effect());
            StartCoroutine(Skill2EffectGround());
        }
        // 스킬 발동 조건을 만족시키고 있지 않을 경우
        else
        {
            audio.clip = audioClip[0];
            audio.Play();
            messageAlpha = 1.0f;
        }
    }

    #endregion

    #region 코루틴 함수

    #region 스킬 1 이펙트 출력 코루틴

    IEnumerator Skill1Effect()
    {
        // 이펙트 출력
        GameObject effect = Instantiate(skill1Effect,
            new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
        effect.transform.SetParent(GameObject.Find("Effects").transform);

        yield return new WaitForSeconds(2.0f);

        Destroy(effect);
    }

    #endregion

    #region 스킬 2 이펙트 출력 코루틴

    IEnumerator Skill2Effect()
    {
        int index = Random.Range(0, 4);

        // 이펙트 출력
        GameObject effect = Instantiate(skill2Effect[index],
            new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
        effect.transform.SetParent(GameObject.Find("Effects").transform);

        yield return new WaitForSeconds(2.0f);

        Destroy(effect);
    }

    IEnumerator Skill2EffectGround()
    {
        // 이펙트 출력
        GameObject effect = Instantiate(skill2Effect[4],
            new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(-90, 0, 0));
        effect.transform.SetParent(GameObject.Find("Effects").transform);
        effect.transform.localScale = new Vector3(6, 6, 6);

        yield return new WaitForSeconds(2.0f);

        Destroy(effect);
    }

    #endregion

    #endregion
}