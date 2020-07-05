using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    #region 컴포넌트 변수 관련

    // 싱글톤에 접근하기 위한 Static 변수
    public static EnemyManager instance = null;
    // 오브젝트 풀에서 생성할 적 몬스터 객체
    public GameObject enemyObject;
    // 자식으로 배치할 부모 오브젝트
    public GameObject enemyParent;
    // 적 몬스터가 출현할 위치를 담을 배열
    public Transform[] enemySpawnPoints;
    // 적 몬스터 오브젝트를 담을 큐
    public Queue<GameObject> enemyPool;

    #endregion

    #region 제어값 변수 관련

    // 적 몬스터를 생성할 주기
    public float enemyCreateTime;
    // 적 몬스터의 최대 생성 개수
    public int enemyMaxCreateCount;
    // 코루틴 함수 실행 및 중지 판별값
    public bool coroutinePlay;
    public bool coroutineStop;

    #endregion

    void Awake()
    {
        #region 싱글톤 변수 할당

        if (instance == null)
        {
            instance = this;
        }
        // instance에 할당된 클래스의 인스턴스가 다를 경우 새로 생성된 클래스를 의미
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        // 다른 씬으로 넘어가더라도 삭제하지 않고 유지
        DontDestroyOnLoad(this.gameObject);

        #endregion

        #region 한 번만 실행할 함수 실행

        // 적 몬스터 오브젝트 풀 생성 함수
        CreateObjectPooling();

        #endregion
    }

    void Start()
    {
        #region 제어값 변수 관련 초기화

        // 적 몬스터를 생성할 주기 초기화
        enemyCreateTime = 1.0f;
        // 적 몬스터의 최대 생성 개수 초기화
        enemyMaxCreateCount = 1000;
        // 코루틴 함수 실행 및 중지 판별값 초기화
        coroutinePlay = false;

        #endregion
    }

    void Update()
    {
        #region 적 몬스터 생성 코루틴 실행

        if (coroutinePlay == true)
        {
            enemySpawnPoints = GameObject.Find("EnemySpawnPoint").GetComponentsInChildren<Transform>();
            if (enemySpawnPoints.Length > 0)
            {
                StartCoroutine(CreateEnemy());
            }
            coroutinePlay = false;
        }

        #endregion

        #region 적 몬스터 생성 코루틴 종료

        if (coroutineStop == true)
        {
            StopAllCoroutines();
            coroutineStop = false;
        }

        #endregion
    }

    #region 적 몬스터 오브젝트 풀 생성 함수

    void CreateObjectPooling()
    {
        enemyPool = new Queue<GameObject>();
        for (int i = 0; i < enemyMaxCreateCount; i++)
        {
            // 불규칙적인 위치 산출
            int idx = Random.Range(1, enemySpawnPoints.Length);
            // 적 캐릭터의 동적 생성
            GameObject enemy = Instantiate(enemyObject);
            // 처음 생성 시 오브젝트 비활성화
            enemy.SetActive(false);
            // 지정한 부모 오브젝트에 자식으로 배치
            enemy.transform.SetParent(enemyParent.transform);
            // 큐에 넣기
            enemyPool.Enqueue(enemy);
        }
    }

    #endregion

    #region 코루틴 함수

    #region 적 캐릭터 생성 코루틴

    IEnumerator CreateEnemy()
    {
        // 게임 종료 시까지 무한 루프
        while (true)
        {
            // 적 캐릭터의 생성 주기 시간만큼 대기
            yield return new WaitForSeconds(enemyCreateTime);

            // 큐에 들어있는 오브젝트가 있을 경우
            if (enemyPool.Count > 0)
            {
                // 큐에서 빼기
                GameObject enemy = enemyPool.Dequeue();
                // 오브젝트 활성화
                enemy.SetActive(true);
                // 컴포넌트 할당
                EnemyInfo enemyInfo = enemy.GetComponent<EnemyInfo>();
                // 적 몬스터 스폰 위치 랜덤 설정
                int index = Random.Range(1, enemySpawnPoints.Length);
                // 적 몬스터 위치 설정
                enemy.transform.position = enemySpawnPoints[index].transform.position;

                #region 적 세부 스탯 조정
                
                switch (GameManager.instance.currentRound)
                {
                    case 1:
                        enemyInfo.enemyMaxHP = Random.Range(80, 100);
                        enemyInfo.enemyCurrentHP = enemyInfo.enemyMaxHP;
                        enemyInfo.enemyAttackMin = 6;
                        enemyInfo.enemyAttackMax = 10;
                        enemyInfo.enemyDefense = 0;
                        enemyInfo.enemySpeed = 5.0f;
                        enemyInfo.enemyGoldMin = 0;
                        enemyInfo.enemyGoldMax = 3;
                        break;
                    case 2:
                        enemyInfo.enemyMaxHP = Random.Range(100, 150);
                        enemyInfo.enemyCurrentHP = enemyInfo.enemyMaxHP;
                        enemyInfo.enemyAttackMin = 10;
                        enemyInfo.enemyAttackMax = 15;
                        enemyInfo.enemyDefense = 2;
                        enemyInfo.enemySpeed = 6.0f;
                        enemyInfo.enemyGoldMin = 1;
                        enemyInfo.enemyGoldMax = 4;
                        break;
                    case 3:
                        enemyInfo.enemyMaxHP = Random.Range(140, 200);
                        enemyInfo.enemyCurrentHP = enemyInfo.enemyMaxHP;
                        enemyInfo.enemyAttackMin = 14;
                        enemyInfo.enemyAttackMax = 20;
                        enemyInfo.enemyDefense = 4;
                        enemyInfo.enemySpeed = 8.0f;
                        enemyInfo.enemyGoldMin = 2;
                        enemyInfo.enemyGoldMax = 5;
                        break;
                    case 4:
                        enemyInfo.enemyMaxHP = Random.Range(180, 280);
                        enemyInfo.enemyCurrentHP = enemyInfo.enemyMaxHP;
                        enemyInfo.enemyAttackMin = 18;
                        enemyInfo.enemyAttackMax = 25;
                        enemyInfo.enemyDefense = 7;
                        enemyInfo.enemySpeed = 10.0f;
                        enemyInfo.enemyGoldMin = 4;
                        enemyInfo.enemyGoldMax = 7;
                        break;
                    case 5:
                        enemyInfo.enemyMaxHP = Random.Range(250, 400);
                        enemyInfo.enemyCurrentHP = enemyInfo.enemyMaxHP;
                        enemyInfo.enemyAttackMin = 22;
                        enemyInfo.enemyAttackMax = 30;
                        enemyInfo.enemyDefense = 10;
                        enemyInfo.enemySpeed = 15.0f;
                        enemyInfo.enemyGoldMin = 6;
                        enemyInfo.enemyGoldMax = 10;
                        break;
                    default:
                        break;
                }

                #endregion
            }
        }
    }

    #endregion

    #endregion
}