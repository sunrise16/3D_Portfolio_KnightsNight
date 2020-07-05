using System.Collections;
using UnityEngine;

public class PlayerSkill1 : MonoBehaviour
{
    #region 컴포넌트 변수 관련

    // 오디오 소스 컴포넌트
    private AudioSource audio;
    public AudioClip[] audioClip;
    // 스킬 폭발 이펙트 오브젝트
    public GameObject skillEffect;
    public GameObject skillEffectGround;

    #endregion

    #region 제어값 변수 관련

    // 스킬 오브젝트 이동 속도
    public float skillSpeed;
    // 스킬 적중 여부 판별값
    public bool isHit;

    #endregion

    void Start()
    {
        #region 컴포넌트 변수 관련 초기화

        // 오디오 소스 컴포넌트 할당
        audio = GetComponent<AudioSource>();

        #endregion

        #region 제어값 변수 관련 초기화

        // 스킬 오브젝트 이동 속도 초기화
        skillSpeed = 10.0f;
        // 스킬 적중 여부 판별값 초기화
        isHit = false;

        #endregion

        #region 코루틴 실행

        // 자동 오브젝트 제거
        StartCoroutine(Destroy());

        #endregion
    }

    void Update()
    {
        #region 스킬 오브젝트 전방 이동

        // 전방으로 지속 이동
        if (isHit == false)
        {
            transform.Translate(Vector3.forward * skillSpeed * Time.deltaTime);
        }

        #endregion

        #region 스킬 적중 시 폭발

        GameObject[] enemy = GameObject.FindGameObjectsWithTag("ENEMY");
        foreach (var enemyObject in enemy)
        {
            float distance = Vector3.Distance(enemyObject.transform.position, transform.position);
            if (distance <= 3.0f && isHit == false)
            {
                // 스킬 적중 판별
                isHit = true;

                // 효과음 출력
                audio.clip = audioClip[0];
                audio.Play();

                Collider[] colls = Physics.OverlapSphere(transform.position, 6.0f, 1 << 10 | 1 << 11);
                foreach (var coll in colls)
                {
                    // 콤보 수 갱신
                    GameObject.Find("Player").GetComponent<PlayerFSM>().playerComboCounting = true;
                    GameObject.Find("Player").GetComponent<PlayerFSM>().comboDelay = 0.0f;
                    DataManager.instance.totalScore += 10 + (DataManager.instance.playerCurrentCombo);
                    DataManager.instance.playerCurrentCombo++;

                    // 적 몬스터 데미지
                    coll.gameObject.GetComponent<EnemyFSM>().HitDamage(DataManager.instance.playerSkill1Damage);
                }

                // 폭발 이펙트 코루틴 실행
                StartCoroutine(SkillExplosionEffect());
                StartCoroutine(SkillExplosionEffectGround());

                // 오브젝트 제거 코루틴 실행
                StartCoroutine(HitDestroy());
            }
        }

        #endregion
    }

    #region 코루틴 함수

    #region 스킬 적중 시 폭발 이펙트 코루틴

    IEnumerator SkillExplosionEffect()
    {
        // 이펙트 출력
        GameObject effect = Instantiate(skillEffect,
            new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
        effect.transform.SetParent(GameObject.Find("Effects").transform);

        yield return new WaitForSeconds(2.0f);

        Destroy(effect);
    }

    IEnumerator SkillExplosionEffectGround()
    {
        // 이펙트 출력
        GameObject effect = Instantiate(skillEffectGround,
            new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.Euler(-90, 0, 0));
        effect.transform.SetParent(GameObject.Find("Effects").transform);
        effect.transform.localScale = new Vector3(6, 6, 6);

        yield return new WaitForSeconds(2.0f);

        Destroy(effect);
    }

    #endregion

    #region 오브젝트 제거 코루틴

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(4.0f);

        Destroy(gameObject);
    }

    IEnumerator HitDestroy()
    {
        yield return new WaitForSeconds(2.0f);

        Destroy(gameObject);
    }

    #endregion

    #endregion
}
