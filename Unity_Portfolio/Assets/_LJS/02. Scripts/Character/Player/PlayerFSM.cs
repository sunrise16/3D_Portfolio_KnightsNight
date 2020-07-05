using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerFSM : MonoBehaviour
{
    #region 컴포넌트 변수 관련

    // 플레이어 상태값 열거문
    public enum PlayerState { Idle, Move, Jump, Attack, MoveAttack, SkillUse, Damaged, Die }
    // 캐릭터 컨트롤러 컴포넌트
    private CharacterController cc;
    // 애니메이션 컴포넌트
    private Animator anim;
    // 오디오 소스 컴포넌트
    private AudioSource audio;
    public AudioClip[] audioClip;
    // 이펙트 오브젝트
    public GameObject[] damagedEffect;
    public GameObject[] dieEffect;

    #endregion

    #region 제어값 변수 관련

    // 플레이어의 상태
    public PlayerState playerState;
    // 플레이어 점프 파워값
    public float playerJumpPower;
    // 플레이어 중력값
    public float playerGravity;
    // 플레이어 행동 제한 판별값
    public bool playerStop;
    // 플레이어 이동 속도
    private float playerSpeed;
    // 낙하 속도값
    private float playerVelocityY;
    // 플레이어 점프 판별값
    private int playerJumpCount;
    // 플레이어 콤보 측정 판별값
    public bool playerComboCounting;
    // 플레이어 콤보 측정 딜레이값
    public float comboDelay;
    // 플레이어 공격 딜레이
    private bool isAttack;
    private float playerAttackDelay;
    // 플레이어 스킬 사용 판별값
    private bool isSkillUse;
    // 플레이어 피격 딜레이
    private bool isDamaged;
    private float playerDamagedDelay;
    // 플레이어 사망 판별값
    private bool isDie;
    // 플레이어 발소리 출력에 필요한 판별값
    private float footstepDelay;
    private bool playerFootstep;
    // 플레이어 체력, 마력 리젠 관련
    private float playerHPRegenDelay;
    private float playerMPRegenDelay;

    #endregion

    void Start()
    {
        #region 컴포넌트 변수 관련 초기화

        // 캐릭터 컨트롤러 컴포넌트 할당
        cc = GetComponent<CharacterController>();
        // 애니메이터 컴포넌트 할당
        anim = GetComponentInChildren<Animator>();
        // 오디오 소스 컴포넌트 할당
        audio = GetComponent<AudioSource>();

        #endregion

        #region 제어값 변수 관련 초기화

        // 플레이어의 상태 초기화
        playerState = PlayerState.Idle;
        // 플레이어 점프 파워값 초기화
        playerJumpPower = 2.0f;
        // 플레이어 중력값 초기화
        playerGravity = -20.0f;
        // 플레이어 행동 제한 판별값 초기화
        playerStop = false;
        // 플레이어 이동 속도 초기화
        playerSpeed = 0.0f;
        // 낙하 속도값 초기화
        playerVelocityY = 0;
        // 플레이어 점프 판별값 초기화
        playerJumpCount = 0;
        // 플레이어 콤보 측정 판별값 초기화
        playerComboCounting = false;
        // 플레이어 콤보 수 측정 딜레이값 초기화
        comboDelay = 0.0f;
        // 플레이어 공격 딜레이 초기화
        isAttack = false;
        playerAttackDelay = 0.0f;
        // 플레이어 스킬 사용 판별값 초기화
        isSkillUse = false;
        // 플레이어 피격 딜레이 초기화
        isDamaged = false;
        playerDamagedDelay = 0.0f;
        // 플레이어 사망 판별값 초기화
        isDie = false;
        // 플레이어 발소리 출력에 필요한 판별값
        footstepDelay = 0.0f;
        playerFootstep = false;
        // 플레이어 체력, 마력 리젠 관련
        playerHPRegenDelay = 0.0f;
        playerMPRegenDelay = 0.0f;

        #endregion
    }

    void Update()
    {
        #region 지속적으로 실행할 함수 실행

        // 플레이어가 사망하지 않은 상태일 때
        if (isDie == false)
        {
            // 상태에 따른 플레이어 행동 처리 함수
            if (isDamaged == false && GameManager.instance.timeRemainSet == true && playerStop == false)
            {
                Move();
                Jump();
                Attack();
            }
            if (playerState == PlayerState.Damaged)
            {
                Damaged();
            }
        }

        #endregion

        #region 실시간 변수값 갱신

        // 플레이어 콤보 측정 딜레이값 증가
        if (playerComboCounting == true)
        {
            comboDelay += Time.deltaTime;

            if (comboDelay >= 5.0f)
            {
                comboDelay = 0.0f;
                playerComboCounting = false;
                DataManager.instance.playerCurrentCombo = 0;
            }
        }
        else
        {
            comboDelay = 0.0f;
        }

        #endregion

        #region 체력, 마력 리젠

        playerHPRegenDelay += Time.deltaTime;
        playerMPRegenDelay += Time.deltaTime;

        if (playerHPRegenDelay >= 2.0f &&
            (DataManager.instance.playerCurrentHP > 0 && DataManager.instance.playerCurrentHP < DataManager.instance.playerMaxHP))
        {
            playerHPRegenDelay = 0.0f;
            DataManager.instance.playerCurrentHP += 1;
        }
        if (playerMPRegenDelay >= 1.0f &&
            (DataManager.instance.playerCurrentMP >= 0 && DataManager.instance.playerCurrentMP < DataManager.instance.playerMaxMP))
        {
            playerMPRegenDelay = 0.0f;
            DataManager.instance.playerCurrentMP += 1;
        }

        #endregion
    }

    #region 플레이어 이동 상태 처리 함수

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0, v);
        // 대각선 이동 속도를 상하좌우 속도와 동일하게 만들기
        // 게임에 따라 일부러 대각선은 빠르게 이동하도록 하는 경우도 존재하므로 이 경우 벡터 정규화를 하지 말 것
        // dir.Normalize();
        // transform.Translate(dir * speed * Time.deltaTime);

        // 카메라가 보는 방향으로 이동해야 함
        dir = Camera.main.transform.TransformDirection(dir);
        // transform.Translate(dir * speed * Time.deltaTime);
        // cc.Move(dir * speed * Time.deltaTime);

        // 중력을 적용시킨 후 이동
        playerVelocityY += playerGravity * (Time.deltaTime * 0.3f);
        dir.y = playerVelocityY;
        cc.Move(dir * DataManager.instance.playerSpeed * Time.deltaTime);
        
        // 플레이어 이동 속도 증감
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            playerSpeed += Time.deltaTime;

            if (playerSpeed >= DataManager.instance.playerSpeed)
            {
                playerSpeed = DataManager.instance.playerSpeed;
            }
        }
        else
        {
            playerSpeed -= Time.deltaTime;

            if (playerSpeed <= 0.0f)
            {
                playerSpeed = 0.0f;
            }
        }

        // 이동 키 입력 시 처리
        if (isAttack == false && (Input.GetButton("Horizontal") || Input.GetButton("Vertical")))
        {
            // 이동 시 플레이어 상태 전환
            if (playerState == PlayerState.Idle)
            {
                anim.SetTrigger("Move");
                playerState = PlayerState.Move;
            }

            // 발소리 출력
            if (playerState == PlayerState.Move)
            {
                footstepDelay += Time.deltaTime;
                if (footstepDelay >= 0.3f)
                {
                    footstepDelay = 0.0f;
                    if (playerFootstep == true)
                    {
                        audio.clip = audioClip[0];
                        audio.Play();
                        playerFootstep = false;
                    }
                    else
                    {
                        audio.clip = audioClip[1];
                        audio.Play();
                        playerFootstep = true;
                    }
                }
            }
        }

        // 멈췄을 때 플레이어 상태를 기본 상태로 전환
        if (playerSpeed <= 0.0f)
        {
            anim.SetTrigger("Idle");
            playerState = PlayerState.Idle;
            footstepDelay = 0.0f;
        }
    }

    #endregion

    #region 플레이어 점프 상태 처리 함수

    void Jump()
    {
        // 캐릭터 점프 (점프 버튼을 누르면 수직속도에 점프 파워를 넣음)
        // 스페이스바 입력 시 점프
        if (Input.GetKeyDown(KeyCode.Space) && playerJumpCount < 1)
        {
            audio.clip = audioClip[2];
            audio.Play();
            playerVelocityY = playerJumpPower;
            playerJumpCount++;
            anim.SetTrigger("Jump");
            playerState = PlayerState.Jump;
        }
        // 땅에 닿으면 낙하 속도와 점프 카운트를 0으로 초기화
        if (playerVelocityY < 0.0f && cc.collisionFlags == CollisionFlags.Below)
        {
            playerVelocityY = 0;
            playerJumpCount = 0;
            if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            {
                anim.SetTrigger("Move");
                playerState = PlayerState.Move;
            }
            else
            {
                anim.SetTrigger("Idle");
                playerState = PlayerState.Idle;
            }
        }
    }

    #endregion

    #region 플레이어 일반 공격 상태 처리 함수

    void Attack()
    {
        // 플레이어 공격 딜레이 적용
        if (isAttack == true)
        {
            playerAttackDelay += Time.deltaTime;
            if (playerAttackDelay >= 0.7f)
            {
                playerAttackDelay = 0.0f;
                isAttack = false;

                // 플레이어 상태값 전환
                if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
                {
                    anim.SetTrigger("Move");
                    playerState = PlayerState.Move;
                }
                else
                {
                    anim.SetTrigger("Idle");
                    playerState = PlayerState.Idle;
                }
            }
        }

        // 마우스 좌클릭 시 플레이어 공격
        if (isAttack == false && (playerState != PlayerState.Jump && isSkillUse == false) && Input.GetMouseButtonDown(0))
        {
            int index = Random.Range(3, 5);
            audio.clip = audioClip[index];
            audio.Play();
            isAttack = true;

            // 주변의 적 몬스터 오브젝트 검출 후 데미지
            Collider[] colls = Physics.OverlapSphere(transform.position, 3.0f, 1 << 10 | 1 << 11);
            foreach (var coll in colls)
            {
                if (coll.GetComponent<EnemyInfo>().enemyCurrentHP > 0)
                {
                    playerComboCounting = true;
                    comboDelay = 0.0f;
                    DataManager.instance.totalScore += 10 + (DataManager.instance.playerCurrentCombo);
                    DataManager.instance.playerCurrentCombo++;
                    coll.GetComponent<EnemyFSM>().HitDamage(DataManager.instance.playerAttack);
                }
            }

            // 플레이어 상태값 전환
            if (playerState == PlayerState.Idle)
            {
                anim.SetTrigger("Attack");
                playerState = PlayerState.Attack;
            }
            else if (playerState == PlayerState.Move)
            {
                anim.SetTrigger("MoveAttack");
                playerState = PlayerState.MoveAttack;
            }
        }
    }

    #endregion

    #region 플레이어 피격 상태 처리 함수

    void Damaged()
    {
        // 플레이어 피격 딜레이 적용
        if (isDamaged == true)
        {
            playerDamagedDelay += Time.deltaTime;

            if (playerDamagedDelay >= 0.5f)
            {
                playerDamagedDelay = 0.0f;
                anim.SetTrigger("Idle");
                playerState = PlayerState.Idle;
                isDamaged = false;
            }
        }
    }

    // 플레이어 HP 감소
    public void PlayerDamage(int value)
    {
        // 플레이어의 HP 감소
        DataManager.instance.playerCurrentHP -= value;

        // 플레이어의 체력이 0 이상일 경우 Damaged 함수 실행
        if (DataManager.instance.playerCurrentHP > 0)
        {
            // 피격 이펙트 출력
            StartCoroutine(DamagedEffect());

            isDamaged = true;
            anim.SetTrigger("Damaged");
            playerState = PlayerState.Damaged;
        }
        // 체력이 0일 경우 Die 함수 실행
        if (DataManager.instance.playerCurrentHP <= 0)
        {
            // 사망 이펙트 출력
            StartCoroutine(DieEffect());

            isDie = true;
            GameManager.instance.isGameOver = true;
            Die();
        }
    }

    #endregion

    #region 플레이어 사망 상태 처리 함수

    void Die()
    {
        // 진행중인 모든 코루틴 정지
        StopAllCoroutines();
        anim.SetTrigger("Die");
        playerState = PlayerState.Die;

        // 사망 상태를 처리하기 위한 코루틴 실행
        StartCoroutine(DieProc());
    }

    // 사망 상태 처리 코루틴
    IEnumerator DieProc()
    {
        // 캐릭터 컨트롤러 비활성화
        cc.enabled = false;

        yield return null;
    }

    #endregion

    #region 플레이어 스킬 사용

    public void PlayerSkillUse()
    {
        StartCoroutine(PlayerSkill());
    }

    #endregion

    #region 코루틴 함수

    #region 플레이어 피격 이펙트 코루틴

    IEnumerator DamagedEffect()
    {
        int index = Random.Range(0, 4);

        // 이펙트 출력
        GameObject effect = Instantiate(damagedEffect[index],
            new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z), Quaternion.identity);
        effect.transform.SetParent(GameObject.Find("Effects").transform);

        yield return new WaitForSeconds(2.0f);

        Destroy(effect);
    }

    #endregion

    #region 플레이어 사망 이펙트 코루틴

    IEnumerator DieEffect()
    {
        int index = Random.Range(0, 2);

        // 이펙트 출력
        GameObject effect = Instantiate(dieEffect[index],
            new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z), Quaternion.identity);
        effect.transform.SetParent(GameObject.Find("Effects").transform);

        yield return new WaitForSeconds(5.0f);

        Destroy(effect);
    }

    #endregion

    #region 플레이어 스킬 사용 시 상태 변경 코루틴

    IEnumerator PlayerSkill()
    {
        isSkillUse = true;
        playerState = PlayerState.SkillUse;
        anim.SetTrigger("SkillUse");

        yield return new WaitForSeconds(2.0f);

        isSkillUse = false;
        playerState = PlayerState.Idle;
        anim.SetTrigger("Idle");
    }

    #endregion

    #endregion
}