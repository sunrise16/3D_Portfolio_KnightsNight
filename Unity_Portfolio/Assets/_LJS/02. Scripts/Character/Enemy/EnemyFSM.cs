using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    #region 컴포넌트 변수 관련

    // 타겟으로 잡을 오브젝트
    private GameObject targetObject;
    // 캐릭터 컨트롤러 컴포넌트
    private CharacterController cc;
    // 애니메이션 제어용 애니메이터 컴포넌트
    private Animator anim;
    // 네비게이션 에이전트 컴포넌트
    private NavMeshAgent agent;
    // 오디오 소스 컴포넌트
    private AudioSource audio;
    public AudioClip[] audioClip;
    // 부모가 될 Canvas 객체
    private Canvas uiCanvas;
    // 이펙트 오브젝트
    public GameObject[] damagedEffect;
    public GameObject[] dieEffect;
    public GameObject[] dieEffectFinal;
    // HP 게이지 오브젝트를 저장할 변수
    private GameObject hpBar;
    public GameObject hpBarPrefab;
    // HP 수치에 따라 fillAmount 속성을 변경할 Image
    private Image hpBarImage;
    // 발견 아이콘 오브젝트를 저장할 변수
    private GameObject findIcon;
    public GameObject findIconPrefab;
    // 사망 시 드랍할 코인 오브젝트
    public GameObject coinObject;

    #endregion

    #region 제어값 변수 관련

    #region 메인 제어 변수

    // 몬스터 상태값 열거문
    public enum EnemyState { Patrol, Move, Attack, Return, Damaged, Die }
    // 몬스터의 상태
    public EnemyState enemyState;
    // 몬스터와 타겟(플레이어) 오브젝트와의 거리값
    private float distance;
    // 몬스터의 공격력
    private int enemyAttackValue;
    // 몬스터의 이동 속도 (NavMeshAgent를 사용할 경우 주석 처리)
    // private float enemyMoveSpeed;
    // HP 게이지의 위치를 보정할 오프셋
    private Vector3 hpBarOffset;
    // 발견 아이콘의 위치를 보정할 오프셋
    private Vector3 findIconOffset;

    #endregion

    #region Idle 상태에 필요한 변수

    // 몬스터의 순찰 지점
    private Vector3 patrolPoint;

    #endregion

    #region Move 상태에 필요한 변수

    // 몬스터와 타겟 간의 방향
    private Vector3 moveDirection;
    // 플레이어 감지 범위
    public float findRange;

    #endregion

    #region Attack 상태에 필요한 변수

    // 타겟 오브젝트의 레이어 값
    private LayerMask targetLayer;
    // 몬스터의 공격 사정거리
    public float attackRange;
    // 몬스터의 공격 딜레이 기준값
    private float attackDelay;
    // 몬스터의 공격 딜레이값
    private float attackDelayTimer;

    #endregion

    #region Return 상태에 필요한 변수

    // Return 상태 제어값
    private bool enemyReturn = false;
    // 몬스터의 최초 스폰 지점
    private Vector3 spawnPoint;
    // 시작 지점에서 최대 이동 가능한 범위
    public float moveRange;

    #endregion

    #region Damaged 상태에 필요한 변수

    // Damaged 상태 판별값
    private bool enemyDamaged = false;

    #endregion

    #region Die 상태에 필요한 변수

    // Die 상태 판별값
    private bool enemyDie = false;

    #endregion

    #endregion

    void Start()
    {
        #region 컴포넌트 변수 관련 초기화

        // 타겟 오브젝트 지정 (플레이어)
        targetObject = GameObject.Find("Player");
        // 캐릭터 컨트롤러 컴포넌트 할당
        cc = GetComponent<CharacterController>();
        // 애니메이션 제어용 애니메이터 컴포넌트 할당
        anim = GetComponentInChildren<Animator>();
        // 네비게이션 에이전트 컴포넌트 할당
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        // 오디오 소스 컴포넌트 할당
        audio = GetComponent<AudioSource>();

        #endregion

        #region 제어값 변수 관련 초기화

        // 몬스터의 상태 초기화
        enemyState = EnemyState.Patrol;
        anim.SetTrigger("Patrol");
        // 몬스터의 최초 스폰 지점 설정
        spawnPoint = transform.position;
        // 몬스터와 타겟(플레이어) 오브젝트와의 거리값 초기화
        distance = 0.0f;
        // 타겟 오브젝트의 레이어 값 초기화
        targetLayer = LayerMask.NameToLayer("PLAYER");
        // HP 게이지 오프셋 초기화
        hpBarOffset = new Vector3(0, 2.2f, 0);
        // 발견 아이콘 오프셋 초기화
        findIconOffset = new Vector3(0, 3.0f, 0);
        // 몬스터의 공격 딜레이 기준값 초기화
        attackDelay = 1.0f;

    #endregion

        #region 일반 함수 실행

        // 적 몬스터 체력바 세팅
        SetHpBar();

        // 적 몬스터 발견 아이콘 세팅
        SetFindIcon();

        #endregion

        #region 코루틴 함수 실행

        // 몬스터 순찰 코루틴
        StartCoroutine(EnemyPatrol());

        #endregion
    }

    void Update()
    {
        #region 실시간 변수값 갱신

        // NavMeshAgent 활성화
        agent.enabled = true;
        // 몬스터와 타겟(플레이어) 오브젝트와의 거리값 갱신
        distance = Vector3.Distance(transform.position, targetObject.transform.position);
        // 몬스터의 공격력 갱신
        enemyAttackValue = gameObject.GetComponent<EnemyInfo>().enemyAttack;
        // 몬스터 HP 게이지의 fillAmount 속성 변경
        hpBarImage.fillAmount = (float)gameObject.GetComponent<EnemyInfo>().enemyCurrentHP / gameObject.GetComponent<EnemyInfo>().enemyMaxHP;

        #endregion

        #region 몬스터 체력바 활성, 비활성화

        if (hpBar != null)
        {
            if (gameObject.GetComponent<EnemyInfo>().enemyCurrentHP > 0.0f && Mathf.Abs(distance) < findRange)
            {
                hpBar.SetActive(true);
            }
            else
            {
                hpBar.SetActive(false);
            }
        }

        #endregion

        #region 몬스터 발견 아이콘 활성, 비활성화

        if (findIcon != null)
        {
            if (gameObject.GetComponent<EnemyInfo>().enemyCurrentHP > 0.0f && (enemyState != EnemyState.Patrol && enemyState != EnemyState.Return))
            {
                findIcon.SetActive(true);
            }
            else
            {
                findIcon.SetActive(false);
            }
        }

        #endregion

        #region 조건에 따른 상태 제어값 설정

        // Return 상태 활성화
        if (Vector3.Distance(transform.position, spawnPoint) >= moveRange)
        {
            enemyReturn = true;
        }
        // 스폰 위치로 돌아온 후 Return 상태 비활성화
        if (enemyReturn == true && Vector3.Distance(transform.position, spawnPoint) <= 0.1f)
        {
            enemyReturn = false;
            enemyState = EnemyState.Patrol;
            anim.SetTrigger("Patrol");
        }

        #endregion

        #region 조건에 따른 몬스터 상태값 설정

        // Return 상태가 활성화일 때
        if (enemyReturn == true)
        {
            enemyState = EnemyState.Return;
            anim.SetTrigger("Return");
        }
        // 그 이외
        else
        {
            if (DataManager.instance.playerCurrentHP <= 0.0f)
            {
                enemyState = EnemyState.Patrol;
                anim.SetTrigger("Patrol");
            }
            else
            {
                if (enemyDamaged == false && enemyDie == false)
                {
                    // 거리값이 findRange 이상일 시 Idle 상태
                    if (Mathf.Abs(distance) >= findRange)
                    {
                        enemyState = EnemyState.Patrol;
                        anim.SetTrigger("Patrol");
                    }
                    // 거리값이 attackRange 이상 findRange 미만일 시 Move 상태
                    else if (Mathf.Abs(distance) >= attackRange && Mathf.Abs(distance) < findRange)
                    {
                        enemyState = EnemyState.Move;
                        anim.SetTrigger("Move");
                    }
                    // 거리값이 attackRange 미만일 시 Attack 상태
                    else if (Mathf.Abs(distance) < attackRange)
                    {
                        enemyState = EnemyState.Attack;
                        anim.SetTrigger("Attack");
                    }
                }
            }
        }

        #endregion

        #region 상태에 따른 몬스터의 행동 처리

        switch (enemyState)
        {
            // Idle 상태 시 행동
            case EnemyState.Patrol:
                Patrol();
                break;
            // Move 상태 시 행동
            case EnemyState.Move:
                Move();
                break;
            // Attack 상태 시 행동
            case EnemyState.Attack:
                Attack();
                break;
            // Return 상태 시 행동
            case EnemyState.Return:
                Return();
                break;
        }

        // 공격 딜레이값 초기화
        if (enemyState != EnemyState.Attack)
        {
            attackDelayTimer = 0.0f;
        }

        #endregion

        #region 사망하지 않고 라운드 종료 시 제거 처리

        if (GameManager.instance.isWaitTime == true)
        {
            gameObject.SetActive(false);
            EnemyManager.instance.enemyPool.Enqueue(gameObject);
            Destroy(hpBar);
            Destroy(findIcon);
        }

        #endregion
    }

    #region 몬스터의 상태값에 따른 행동 실행 함수

    private void Patrol()
    {
        // 1. 플레이어가 일정 범위 내로 들어오면 이동 상태로 변경
        // 2. 플레이어 찾기 (GameObject.Find("Player"))
        // 탐지 범위 : 12.0f 이상

        // 몬스터의 이동 속도 재설정
        // enemyMoveSpeed = 2.0f;

        // 순찰 지점을 향해 바라보기
        // transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(patrolDirection), 10.0f * Time.deltaTime);

        // 순찰 지점을 향해 이동
        // cc.SimpleMove(patrolDirection * enemyMoveSpeed);
        agent.SetDestination(patrolPoint);
        agent.speed = 2.0f;

        anim.SetTrigger("Move");
    }
    private void Move()
    {
        // 1. 플레이어를 향해 이동 후 공격 범위 안에 들어오면 공격 상태로 변경
        // 2. 플레이어를 추격하더라도 처음 위치에서 일정 범위를 넘어가지 않도록 조치
        // 3. 플레이어처럼 캐릭터 컨트롤러를 이용
        // 탐지 범위 : 1.5f 이상 ~ 12.0f 미만

        // 몬스터의 이동 속도 재설정
        // enemyMoveSpeed = GetComponent<EnemyInfo>().enemySpeed;

        // 타겟의 방향 구하기
        // moveDirection = (targetObject.transform.position - transform.position).normalized;

        // 타겟을 향해 바라보기
        // transform.LookAt(targetObject.transform);
        // transform.forward = Vector3.Lerp(transform.position, moveDirection, 1.0f * Time.deltaTime);
        // transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDirection), 10.0f * Time.deltaTime);

        // 타겟을 향해 이동
        // cc.SimpleMove(moveDirection * enemyMoveSpeed);
        agent.SetDestination(targetObject.transform.position);
        agent.speed = GetComponent<EnemyInfo>().enemySpeed;
    }
    private void Attack()
    {
        // 1. 플레이어가 공격 범위 안에 있다면 일정한 시간 간격으로 플레이어를 공격
        // 2. 플레이어가 공격 범위를 벗어나면 이동 상태(재추격)로 변경
        // 탐지 범위 : 0.0f 이상 ~ 1.5f 미만

        if (gameObject.GetComponent<EnemyInfo>().enemyCurrentHP > 0.0f)
        {
            // 몬스터의 공격 딜레이값 증가
            attackDelayTimer += Time.deltaTime;

            // 공격 딜레이값이 기준값을 넘어섰을 경우
            if (attackDelayTimer > attackDelay)
            {
                // 타겟을 향해 바라보기
                transform.LookAt(targetObject.transform.position);

                // 플레이어 체력 감소
                if (distance <= attackRange)
                {
                    // 효과음 출력
                    audio.clip = audioClip[0];
                    audio.Play();

                    // 플레이어 체력 감소 및 현재 콤보 끊기
                    int damage = enemyAttackValue - DataManager.instance.playerDefense;
                    if (damage < 0)
                    {
                        damage = 0;
                    }
                    GameObject.Find("Player").GetComponent<PlayerFSM>().PlayerDamage(damage);
                    GameObject.Find("Player").GetComponent<PlayerFSM>().playerComboCounting = false;
                    DataManager.instance.playerCurrentCombo = 0;
                    if (DataManager.instance.playerCurrentHP <= 0)
                    {
                        DataManager.instance.playerCurrentHP = 0;
                    }
                }

                // 타이머 초기화
                attackDelayTimer = -0.5f;

                if (Vector3.Distance(transform.position, targetObject.transform.position) > attackRange)
                {
                    enemyState = EnemyState.Move;
                }
            }
        }
    }
    private void Return()
    {
        // 1. 몬스터가 플레이어를 추적하다가 너무 멀리 벗어났을 경우 최초 스폰 위치로 복귀
        // 탐지 범위 : 최초 스폰 위치에서 30.0f 이상

        // 몬스터의 이동 속도 재설정
        // enemyMoveSpeed = 8.0f;

        // 타겟의 방향 구하기
        // moveDirection = (spawnPoint - transform.position).normalized;

        // 목표 지점을 향해 바라보기
        // transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDirection), 10.0f * Time.deltaTime);

        // 최초 스폰 위치를 향해 이동
        // cc.SimpleMove(moveDirection * enemyMoveSpeed);
        agent.SetDestination(spawnPoint);
        agent.speed = 8.0f;
    }
    // 몬스터 피격 시 데미지 계산 함수
    public void HitDamage(int value)
    {
        // 몬스터 체력 감소
        gameObject.GetComponent<EnemyInfo>().enemyCurrentHP -= value;

        // 몬스터의 체력이 1 이상일 경우 피격 상태로 전환
        if (gameObject.GetComponent<EnemyInfo>().enemyCurrentHP > 0)
        {
            enemyDamaged = true;
            if (enemyState != EnemyState.Attack)
            {
                enemyState = EnemyState.Damaged;
                anim.SetTrigger("Damaged");
            }
            Damaged();
        }
        // 몬스터의 체력이 0일 경우 사망 상태로 전환
        else
        {
            enemyDie = true;
            enemyState = EnemyState.Die;
            anim.SetTrigger("Die");
            Die();
        }
    }
    private void Damaged()
    {
        // 1. 몬스터의 체력이 1 이상일 경우 피격 상태로 전환 후 다시 이전의 상태로 변경
        // 2. 트랜지션은 Any State 에서 연결

        // 피격 상태를 처리하기 위한 코루틴 실행
        StartCoroutine(DamageProc());

        // 피격 이펙트 출력
        StartCoroutine(DamagedEffect());
    }
    private void Die()
    {
        // 1. 몬스터의 체력이 0일 경우 사망 상태로 전환, 그 후 몬스터 오브젝트 삭제
        // 2. 트랜지션은 Any State 에서 연결

        // 플레이어 킬 카운트 증가
        DataManager.instance.playerCurrentKillCount++;
        DataManager.instance.playerMaxKillCount++;

        // 퀘스트 1 킬 카운트 처리
        if (DataManager.instance.quest1.isAccepted == true)
        {
            DataManager.instance.quest1.enemyKillCount++;
        }

        // 플레이어 스코어 증가
        DataManager.instance.totalScore += (100 + (25 * (GameManager.instance.currentRound - 1)));

        // 진행중인 모든 코루틴 정지
        // StopAllCoroutines();

        // 사망 상태를 처리하기 위한 코루틴 실행
        StartCoroutine(DieProc());

        // 사망 이펙트 출력
        StartCoroutine(DieEffect());
    }

    #endregion

    #region 적 몬스터 체력바 세팅

    void SetHpBar()
    {
        uiCanvas = GameObject.Find("UI Canvas").GetComponent<Canvas>();
        // UI Canvas 하위로 HP 게이지 생성
        hpBar = Instantiate(hpBarPrefab, uiCanvas.transform);
        // fillAmount 속성을 변경할 Image를 추출
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[1];

        // HP 게이지가 따라가야 할 대상과 오프셋 값 설정
        var _hpBar = hpBar.GetComponent<ImageOutput>();
        _hpBar.targetTr = this.gameObject.transform;
        _hpBar.offset = hpBarOffset;

        hpBar.SetActive(false);
    }

    #endregion

    #region 적 몬스터 발견 아이콘 세팅

    void SetFindIcon()
    {
        uiCanvas = GameObject.Find("UI Canvas").GetComponent<Canvas>();
        // UI Canvas 하위로 HP 게이지 생성
        findIcon = Instantiate(findIconPrefab, uiCanvas.transform);

        // 발견 아이콘이 따라가야 할 대상과 오프셋 값 설정
        var _findIcon = findIcon.GetComponent<ImageOutput>();
        _findIcon.targetTr = this.gameObject.transform;
        _findIcon.offset = findIconOffset;

        findIcon.SetActive(false);
    }

    #endregion

    #region 코루틴 함수

    #region 몬스터 순찰 코루틴

    IEnumerator EnemyPatrol()
    {
        if (enemyState == EnemyState.Patrol)
        {
            while (true)
            {
                // 대기 시간 랜덤 설정
                float waitSecond = Random.Range(4.0f, 6.0f);
                // 순찰할 지점 랜덤 설정
                patrolPoint = new Vector3(transform.position.x + Random.Range(-15.0f, 15.0f), 0, transform.position.z + Random.Range(-15.0f, 15.0f));

                // 랜덤 설정된 waitSecond 초 대기
                yield return new WaitForSeconds(waitSecond);
            }
        }
    }

    #endregion

    #region 몬스터 피격 데미지 코루틴

    IEnumerator DamageProc()
    {
        // 효과음 출력
        audio.clip = audioClip[1];
        audio.Play();

        // 피격 모션 시간만큼 대기
        yield return new WaitForSeconds(1.0f);

        enemyDamaged = false;
        // 현재 상태를 이동으로 전환
        enemyState = EnemyState.Move;
    }

    #endregion

    #region 몬스터 피격 이펙트 코루틴

    IEnumerator DamagedEffect()
    {
        int index = Random.Range(0, 4);

        // 이펙트 출력
        GameObject effect = Instantiate(damagedEffect[index],
            new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z), Quaternion.identity);
        effect.transform.SetParent(GameObject.Find("Effects").transform);

        yield return new WaitForSeconds(1.0f);

        Destroy(effect);
    }

    #endregion

    #region 몬스터 사망 코루틴

    IEnumerator DieProc()
    {
        // 효과음 출력
        audio.clip = audioClip[2];
        audio.Play();

        // 1.5초 대기
        yield return new WaitForSeconds(1.5f);

        // 체력바, 아이콘 숨기기 및 최종 사망 이펙트 출력
        StartCoroutine(DieEffectFinal());
        enemyDie = false;

        // 코인 드랍
        if (gameObject.GetComponent<EnemyInfo>().enemyGold != 0)
        {
            for (int i = 0; i < gameObject.GetComponent<EnemyInfo>().enemyGold; i++)
            {
                GameObject coin = Instantiate(coinObject, new Vector3(transform.position.x + (1.0f * i),
                    transform.position.y + 1.0f, transform.position.z + (1.0f * i)), transform.rotation);
                float force = Random.Range(2.0f, 4.0f);
                Vector3 direction = new Vector3(force, 12, force);
                coin.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
                coin.transform.SetParent(GameObject.Find("CoinPool").transform);
            }
        }
    }

    #endregion

    #region 몬스터 사망 이펙트 코루틴

    IEnumerator DieEffect()
    {
        int index = Random.Range(0, 4);

        // 이펙트 출력
        GameObject effect = Instantiate(dieEffect[index],
            new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z), Quaternion.identity);
        effect.transform.SetParent(GameObject.Find("Effects").transform);

        yield return new WaitForSeconds(2.0f);

        Destroy(effect);
    }

    #endregion

    #region 몬스터 최종 사망 이펙트 코루틴

    IEnumerator DieEffectFinal()
    {
        int index = Random.Range(0, 4);

        // 이펙트 출력
        GameObject effect = Instantiate(dieEffectFinal[index],
            new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z), Quaternion.identity);
        effect.transform.SetParent(GameObject.Find("Effects").transform);

        yield return new WaitForSeconds(1.5f);

        Destroy(effect);
        Destroy(hpBar);
        Destroy(findIcon);
        gameObject.SetActive(false);
        EnemyManager.instance.enemyPool.Enqueue(gameObject);
    }

    #endregion

    #endregion

    #region 기즈모 출력

    private void OnDrawGizmos()
    {
        // 공격 가능한 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // 플레이어 추적 범위
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, findRange);

        // 이동 가능한 최대 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(spawnPoint, moveRange);
    }

    #endregion
}