using LeoLuz.Extensions;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region 컴포넌트 변수 관련

    // 싱글톤에 접근하기 위한 Static 변수
    public static GameManager instance = null;

    // 오디오 소스 컴포넌트
    private AudioSource audio;
    public AudioClip[] audioClip;

    // 게임 오버 시 활성화 시킬 이미지 오브젝트
    public GameObject activeImage1;
    public GameObject activeImage2;
    public GameObject activeImage3;

    // 라운드 시작 시 비활성화 할 NPC 오브젝트
    public GameObject npcPreist;
    public GameObject npcPeasant;

    // 기타 이미지 컴포넌트
    public GameObject waitTimeIcon;
    public GameObject roundTimeIcon;
    public GameObject infoTextAwake;
    public GameObject infoText1;
    public GameObject infoText2;
    public GameObject resultImage;

    #endregion

    #region 제어값 변수 관련

    // 게임 종료 여부 판단값
    public bool isGameOver;
    // 게임 일시 정지 여부 판단값
    public bool isPaused;
    // 게임 종료 여부 딜레이값
    public float gameOverDelay;

    // 현재 라운드
    public int currentRound;
    // 라운드 잔여 대기 시간
    public float timeRemaining;
    // 라운드 잔여 대기 시간 일시 정지 판별값
    public bool timeRemainSet;
    // 현재 라운드 종류 판별값 (대기 타임, 라운드 진행중)
    public bool isWaitTime;
    public bool isRoundTime;

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
    }

    void Start()
    {
        #region 컴포넌트 변수 관련 초기화

        // 오디오 소스 컴포넌트 할당
        audio = GetComponent<AudioSource>();

        #endregion

        #region 제어값 변수 관련 초기화

        // 게임 종료 여부 판단값 초기화
        isGameOver = false;
        // 게임 일시 정지 여부 판단값 초기화
        isPaused = false;
        // 게임 종료 여부 딜레이값 초기화
        gameOverDelay = 0.0f;

        // 현재 라운드 초기화
        currentRound = 1;
        // 라운드 잔여 대기 시간 초기화
        timeRemaining = 120.0f;
        timeRemainSet = true;
        // 현재 라운드 종류 판별값 초기화
        isWaitTime = true;
        isRoundTime = false;

        // 커서 중앙에 고정
        Cursor.lockState = CursorLockMode.Locked;

        #endregion

        #region 코루틴 실행

        StartCoroutine(SwitchTime());

        #endregion
    }

    void Update()
    {
        #region 지속적으로 실행할 함수 실행

        // 게임 오버 처리
        if (isGameOver == true)
        {
            GameOver();
        }

        #endregion
    }
    
    #region 게임 오버 처리 함수

    public void GameOver()
    {
        gameOverDelay += Time.deltaTime;
        Time.timeScale = 0.3f;
        activeImage1.SetActive(true);
        
        if (gameOverDelay >= 0.7f)
        {
            activeImage2.SetActive(true);
        }
        if (gameOverDelay >= 1.4f)
        {
            // 데이터 저장
            PlayerPrefs.SetInt("Player_KillCount", DataManager.instance.playerMaxKillCount);
            PlayerPrefs.SetInt("Player_MaxCombo", DataManager.instance.playerMaxCombo);
            PlayerPrefs.SetInt("Player_GoldRemain", DataManager.instance.playerGold);
            PlayerPrefs.SetInt("Player_TotalScore", DataManager.instance.totalScore);

            activeImage3.SetActive(true);
            InGameFadeImage.instance.alphaChange = false;
        }
    }

    #endregion

    #region 메세지 알파값 조절

    void MessageAlpha(GameObject text)
    {
        text.GetComponent<Text>().FadeOut(10.0f);
    }

    #endregion

    #region 라운드 결산 창 종료

    public void ExitResult()
    {
        // 5라운드 이하일 경우 계속 진행
        if (currentRound <= 4)
        {
            Cursor.lockState = CursorLockMode.Locked;

            // 라운드 증가
            currentRound++;

            // 라운드 잔여 대기 시간 일시 정지 해제
            timeRemainSet = true;

            // 플레이어 라운드 킬 수 초기화
            DataManager.instance.playerCurrentKillCount = 0;

            // 결산 창 닫기
            resultImage.SetActive(false);

            // NPC 활성화
            npcPreist.SetActive(true);
            npcPeasant.SetActive(true);

            // 효과음 출력
            audio.clip = audioClip[2];
            audio.Play();

            // 텍스트 출력
            infoText2.SetActive(false);
            infoText1.SetActive(true);
            MessageAlpha(infoText1);
        }
        // 5라운드 에서 결과창 클릭 시 리절트 씬으로 넘기기
        else
        {
            // 효과음 출력
            audio.clip = audioClip[2];
            audio.Play();

            // 데이터 저장
            PlayerPrefs.SetInt("Player_KillCount", DataManager.instance.playerMaxKillCount);
            PlayerPrefs.SetInt("Player_MaxCombo", DataManager.instance.playerMaxCombo);
            PlayerPrefs.SetInt("Player_GoldRemain", DataManager.instance.playerGold);
            PlayerPrefs.SetInt("Player_TotalScore", DataManager.instance.totalScore);

            Time.timeScale = 0.5f;
            activeImage3.SetActive(true);
            InGameFadeImage.instance.alphaChange = false;
        }
    }

    #endregion

    #region 코루틴 함수

    #region 현재 라운드 종류 설정 코루틴

    IEnumerator SwitchTime()
    {
        // 게임 오버될 때까지 반복
        while (isGameOver == false)
        {
            // 잔여 시간 감소
            if (timeRemainSet == true)
            {
                timeRemaining -= 1.0f;
            }

            // 1초 대기
            yield return new WaitForSeconds(1.0f);
            
            // 잔여 시간이 다했을 경우 라운드 종류 스위칭
            if (timeRemaining <= 0.0f)
            {
                // 전투 라운드로 변경
                if (isWaitTime == true)
                {
                    // 효과음 출력
                    audio.clip = audioClip[0];
                    audio.Play();

                    // 상점, 퀘스트 관련 UI 비활성화
                    for (int i = 0; i < 4; i++)
                    {
                        GameObject.Find("Canvas").transform.GetChild(17 + i).gameObject.SetActive(false);
                    }
                    GameObject.Find("Player").GetComponent<PlayerFSM>().playerStop = false;
                    GameObject.Find("UI Canvas").transform.GetChild(0).gameObject.SetActive(false);
                    GameObject.Find("UI Canvas").transform.GetChild(1).gameObject.SetActive(false);
                    GameObject.Find("NPC").transform.GetChild(0).gameObject.GetComponent<NPCMenu>().MenuActivate(false);
                    GameObject.Find("NPC").transform.GetChild(1).gameObject.GetComponent<NPCMenu>().MenuActivate(false);

                    timeRemaining = 180.0f;
                    isWaitTime = false;
                    isRoundTime = true;
                    waitTimeIcon.SetActive(false);
                    roundTimeIcon.SetActive(true);
                    if (currentRound == 1)
                    {
                        infoTextAwake.SetActive(false);
                    }
                    else
                    {
                        infoText1.SetActive(false);
                    }
                    infoText2.SetActive(true);
                    npcPreist.SetActive(false);
                    npcPeasant.SetActive(false);
                    MessageAlpha(infoText2);
                    EnemyManager.instance.coroutinePlay = true;
                }
                // 대기 라운드로 변경
                else if (isRoundTime == true)
                {
                    // 효과음 출력
                    audio.clip = audioClip[1];
                    audio.Play();

                    // 남아 있는 이펙트 전체 제거
                    GameObject effectParent = GameObject.Find("Effects");
                    for (int i = 0; i < effectParent.transform.childCount; i++)
                    {
                        Destroy(effectParent.transform.GetChild(i).gameObject);
                    }
                    timeRemaining = 120.0f;
                    isWaitTime = true;
                    isRoundTime = false;
                    timeRemainSet = false;
                    waitTimeIcon.SetActive(true);
                    roundTimeIcon.SetActive(false);
                    resultImage.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    EnemyManager.instance.coroutineStop = true;

                    // 퀘스트 클리어 여부 체크
                    #region 퀘스트 1

                    if (DataManager.instance.quest1.isAccepted == true)
                    {
                        if (DataManager.instance.quest1.enemyKillCount >= 50)
                        {
                            DataManager.instance.Quest1Clear();
                        }
                        else
                        {
                            DataManager.instance.Quest1Failed();
                        }
                    }

                    #endregion
                    #region 퀘스트 2

                    if (DataManager.instance.quest2.isAccepted == true)
                    {
                        if (DataManager.instance.quest2.playerDamagedCount <= 15)
                        {
                            DataManager.instance.Quest2Clear();
                        }
                        else
                        {
                            DataManager.instance.Quest2Failed();
                        }
                    }

                    #endregion

                    // 점수 결산
                    DataManager.instance.totalScore += ((DataManager.instance.playerCurrentKillCount * 50) +
                        (DataManager.instance.playerMaxCombo * 100) + (DataManager.instance.playerGold * 10));
                }
            }
        }
    }

    #endregion

    #endregion
}