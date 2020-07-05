using System.Collections;
using UnityEngine;

#region 퀘스트 내부 구성

public class QuestCondition
{
    // 퀘스트 진행 분류 열거문
    public enum QuestType { None, Ongoing, Failed, Completed }
    // 퀘스트 진행 분류 판별값
    public QuestType questState;
    // 퀘스트 수락 여부 판별값
    public bool isAccepted;
    // 퀘스트 보상 획득 가능 여부 판별값
    public bool getReward;
}

#endregion

#region 퀘스트 목록

public class Quest1 : QuestCondition
{
    // 퀘스트 클리어 조건값
    public int enemyKillCount;
}

public class Quest2 : QuestCondition
{
    // 퀘스트 클리어 조건값
    public int playerDamagedCount;
}

#endregion

public class DataManager : MonoBehaviour
{
    #region 컴포넌트 변수 관련

    // 싱글톤에 접근하기 위한 Static 변수
    public static DataManager instance = null;
    // 오디오 소스 컴포넌트
    private AudioSource audio;
    public AudioClip[] audioClip;
    // 상점 구입 시 출력할 이펙트
    public GameObject[] purchaseEffect;
    public GameObject[] blastEffect;
    // 콤보 메세지 오브젝트
    public GameObject comboMessage;
    // 구입 불가 메세지 오브젝트
    public GameObject cannotPurchaseText;

    #region 퀘스트 관련

    // 퀘스트 1 컴포넌트
    public Quest1 quest1;
    // 퀘스트 2 컴포넌트
    public Quest2 quest2;

    #endregion

    #endregion

    #region 제어값 변수 관련

    #region 플레이어 관련

    // 플레이어의 현재 HP
    public int playerCurrentHP;
    // 플레이어의 최대 HP
    public int playerMaxHP;
    // 플레이어의 현재 MP
    public int playerCurrentMP;
    // 플레이어의 최대 MP
    public int playerMaxMP;
    // 플레이어의 최소 공격력
    public int playerAttackMin;
    // 플레이어의 최대 공격력
    public int playerAttackMax;
    // 플레이어의 공격력 (최소 ~ 최대 사이값)
    public int playerAttack;
    // 플레이어의 방어력
    public int playerDefense;
    // 플레이어의 이동 속도
    public float playerSpeed;

    #endregion

    #region 스킬 관련

    // 플레이어 스킬 1 최소 데미지
    public int playerSkill1DamageMin;
    // 플레이어 스킬 1 최대 데미지
    public int playerSkill1DamageMax;
    // 플레이어 스킬 1 데미지 (최소 ~ 최대 사이값)
    public int playerSkill1Damage;
    // 플레이어 스킬 2 최소 데미지
    public int playerSkill2DamageMin;
    // 플레이어 스킬 2 최대 데미지
    public int playerSkill2DamageMax;
    // 플레이어 스킬 2 데미지 (최소 ~ 최대 사이값)
    public int playerSkill2Damage;
    // 플레이어 스킬 1 쿨타임
    public int playerSkill1Cooldown;
    public float skill1CooldownDelay;
    // 플레이어 스킬 2 쿨타임
    public int playerSkill2Cooldown;
    public float skill2CooldownDelay;
    // 플레이어 스킬 쿨타임 딜레이값
    private float playerSkillCooldownDelay;

    #endregion

    #region 시스템 관련

    // 플레이어의 소지 금액
    public int playerGold;
    // 토탈 스코어
    public int totalScore;
    // 플레이어의 현재 킬 수
    public int playerCurrentKillCount;
    // 플레이어의 최대 킬 수
    public int playerMaxKillCount;
    // 플레이어의 현재 콤보 수
    public int playerCurrentCombo;
    // 플레이어의 최대 콤보 수
    public int playerMaxCombo;
    // 상점에서의 플레이어 체력 구입 가격
    public int playerHPPrice;
    // 상점에서의 플레이어 마력 구입 가격
    public int playerMPPrice;
    // 상점에서의 플레이어 공격력 구입 가격
    public int playerAttackPrice;
    // 상점에서의 플레이어 방어력 구입 가격
    public int playerDefensePrice;
    // 상점에서의 플레이어 스피드 구입 가격
    public int playerSpeedPrice;

    #endregion

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

        #region 퀘스트 관련 초기화

        // 클래스 동적 생성
        quest1 = new Quest1();
        quest2 = new Quest2();

        // 퀘스트 1 초기화
        quest1.questState = QuestCondition.QuestType.None;
        quest1.isAccepted = false;
        quest1.getReward = false;
        quest1.enemyKillCount = 0;
        // 퀘스트 2 초기화
        quest2.questState = QuestCondition.QuestType.None;
        quest2.isAccepted = false;
        quest2.getReward = false;
        quest2.playerDamagedCount = 0;

        #endregion

        #endregion

        #region 제어값 변수 관련 초기화

        // 플레이어 관련
        playerCurrentHP = 200;
        playerMaxHP = 200;
        playerCurrentMP = 100;
        playerMaxMP = 100;
        playerAttackMin = 10;
        playerAttackMax = 20;
        playerDefense = 0;
        playerSpeed = 5.0f;

        // 스킬 관련
        playerSkill1DamageMin = 200;
        playerSkill1DamageMax = 400;
        playerSkill2DamageMin = 300;
        playerSkill2DamageMax = 600;
        playerSkill1Cooldown = 0;
        skill1CooldownDelay = 0.0f;
        playerSkill2Cooldown = 0;
        skill2CooldownDelay = 0.0f;
        playerSkillCooldownDelay = 0.0f;

        // 시스템 관련
        playerGold = 10;
        totalScore = 0;
        playerCurrentKillCount = 0;
        playerMaxKillCount = 0;
        playerCurrentCombo = 0;
        playerMaxCombo = 0;
        playerHPPrice = 1;
        playerMPPrice = 1;
        playerAttackPrice = 1;
        playerDefensePrice = 1;
        playerSpeedPrice = 1;

        #endregion
    }

    void Update()
    {
        #region 실시간 변수값 갱신

        // 플레이어 공격 데미지 랜덤 지정
        playerAttack = Random.Range(playerAttackMin, playerAttackMax + 1);

        // 플레이어 스킬 1 데미지 랜덤 지정
        playerSkill1Damage = Random.Range(playerSkill1DamageMin, playerSkill1DamageMax);

        // 플레이어 스킬 2 데미지 랜덤 지정
        playerSkill2Damage = Random.Range(playerSkill2DamageMin, playerSkill2DamageMax);

        // 플레이어 스킬 쿨타임 갱신
        playerSkillCooldownDelay += Time.deltaTime;
        if (playerSkillCooldownDelay >= 1.0f)
        {
            playerSkillCooldownDelay = 0.0f;
            playerSkill1Cooldown--;
            if (playerSkill1Cooldown <= 0)
            {
                playerSkill1Cooldown = 0;
            }
            playerSkill2Cooldown--;
            if (playerSkill2Cooldown <= 0)
            {
                playerSkill2Cooldown = 0;
            }
        }
        skill1CooldownDelay -= Time.deltaTime;
        if (skill1CooldownDelay <= 0.0f)
        {
            skill1CooldownDelay = 0.0f;
        }
        skill2CooldownDelay -= Time.deltaTime;
        if (skill2CooldownDelay <= 0.0f)
        {
            skill2CooldownDelay = 0.0f;
        }

        // 플레이어의 최대 콤보 수 갱신
        if (playerCurrentCombo > playerMaxCombo)
        {
            playerMaxCombo = playerCurrentCombo;
        }

        #endregion

        #region 콤보 메세지 출력

        if (GameObject.Find("Player").GetComponent<PlayerFSM>().playerComboCounting == true)
        {
            comboMessage.SetActive(true);
        }
        else
        {
            comboMessage.SetActive(false);
        }

        #endregion
    }

    #region 상점 구입 및 상점 가격 증가

    public void PlayerHPPurchase()
    {
        if (playerGold >= playerHPPrice)
        {
            audio.clip = audioClip[0];
            audio.Play();
            playerMaxHP += 5;
            playerGold -= playerHPPrice;
            playerHPPrice++;
            StartCoroutine(PurchaseEffect(0));
            StartCoroutine(BlastEffect(0));
        }
        else
        {
            audio.clip = audioClip[1];
            audio.Play();
            cannotPurchaseText.GetComponent<CannotPurchase>().textAlpha = 1.0f;
        }
    }

    public void PlayerMPPurchase()
    {
        if (playerGold >= playerMPPrice)
        {
            audio.clip = audioClip[0];
            audio.Play();
            playerMaxMP += 5;
            playerGold -= playerMPPrice;
            playerMPPrice++;
            StartCoroutine(PurchaseEffect(1));
            StartCoroutine(BlastEffect(1));
        }
        else
        {
            audio.clip = audioClip[1];
            audio.Play();
            cannotPurchaseText.GetComponent<CannotPurchase>().textAlpha = 1.0f;
        }
    }

    public void PlayerAttackPurchase()
    {
        if (playerGold >= playerAttackPrice)
        {
            audio.clip = audioClip[0];
            audio.Play();
            playerAttackMin += 2;
            playerAttackMax += 3;
            playerGold -= playerAttackPrice;
            playerAttackPrice++;
            StartCoroutine(PurchaseEffect(2));
            StartCoroutine(BlastEffect(2));
        }
        else
        {
            audio.clip = audioClip[1];
            audio.Play();
            cannotPurchaseText.GetComponent<CannotPurchase>().textAlpha = 1.0f;
        }
    }

    public void PlayerDefensePurchase()
    {
        if (playerGold >= playerDefensePrice)
        {
            audio.clip = audioClip[0];
            audio.Play();
            playerDefense++;
            playerGold -= playerDefensePrice;
            playerDefensePrice++;
            StartCoroutine(PurchaseEffect(3));
            StartCoroutine(BlastEffect(3));
        }
        else
        {
            audio.clip = audioClip[1];
            audio.Play();
            cannotPurchaseText.GetComponent<CannotPurchase>().textAlpha = 1.0f;
        }
    }

    public void PlayerSpeedPurchase()
    {
        if (playerGold >= playerSpeedPrice)
        {
            audio.clip = audioClip[0];
            audio.Play();
            playerSpeed += 0.5f;
            playerGold -= playerSpeedPrice;
            playerSpeedPrice++;
            StartCoroutine(PurchaseEffect(4));
            StartCoroutine(BlastEffect(4));
        }
        else
        {
            audio.clip = audioClip[1];
            audio.Play();
            cannotPurchaseText.GetComponent<CannotPurchase>().textAlpha = 1.0f;
        }
    }

    #endregion

    #region 퀘스트 관련

    #region 퀘스트 수락

    public void Quest1Accept()
    {
        if (quest1.isAccepted == false && quest1.questState == QuestCondition.QuestType.None)
        {
            audio.clip = audioClip[2];
            audio.Play();
            quest1.isAccepted = true;
            quest1.questState = QuestCondition.QuestType.Ongoing;
        }
    }

    public void Quest2Accept()
    {
        if (quest2.isAccepted == false && quest2.questState == QuestCondition.QuestType.None)
        {
            audio.clip = audioClip[2];
            audio.Play();
            quest2.isAccepted = true;
            quest2.questState = QuestCondition.QuestType.Ongoing;
        }
    }

    #endregion

    #region 퀘스트 클리어

    public void Quest1Clear()
    {
        quest1.isAccepted = false;
        quest1.getReward = true;
        quest1.questState = QuestCondition.QuestType.Completed;
    }

    public void Quest2Clear()
    {
        quest2.isAccepted = false;
        quest2.getReward = true;
        quest2.questState = QuestCondition.QuestType.Completed;
    }

    #endregion

    #region 퀘스트 실패

    public void Quest1Failed()
    {
        quest1.isAccepted = false;
        quest1.questState = QuestCondition.QuestType.Failed;
    }

    public void Quest2Failed()
    {
        quest2.isAccepted = false;
        quest2.questState = QuestCondition.QuestType.Failed;
    }

    #endregion

    #endregion

    #region 코루틴 함수

    #region 상점 구입 이펙트 코루틴

    IEnumerator PurchaseEffect(int index)
    {
        // 플레이어 지정
        GameObject player = GameObject.Find("Player");

        // 이펙트 출력
        GameObject effect = Instantiate(purchaseEffect[index], new Vector3(player.transform.position.x,
            player.transform.position.y + 1.0f, player.transform.position.z), purchaseEffect[index].transform.rotation);
        effect.transform.SetParent(GameObject.Find("Effects").transform);

        yield return new WaitForSeconds(2.0f);

        Destroy(effect);
    }
    IEnumerator BlastEffect(int index)
    {
        // 플레이어 지정
        GameObject player = GameObject.Find("Player");

        // 이펙트 출력
        GameObject effect = Instantiate(blastEffect[index],
            new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z), blastEffect[index].transform.rotation);
        effect.transform.SetParent(GameObject.Find("Effects").transform);

        yield return new WaitForSeconds(2.0f);

        Destroy(effect);
    }

    #endregion

    #endregion
}