using UnityEngine;
using UnityEngine.UI;

public class KillCountResult : MonoBehaviour
{
    #region 컴포넌트 변수 관련

    // 킬 카운트 결산 텍스트 컴포넌트
    public Text killCountResultText;
    // 보너스 점수를 담을 텍스트
    public GameObject bonusScoreText;

    #endregion

    void Start()
    {
        #region 컴포넌트 변수 관련 초기화

        // 킬 카운트 결산 텍스트 컴포넌트 할당
        killCountResultText = GetComponent<Text>();

        #endregion
    }

    void Update()
    {
        #region 제어값 변수 갱신

        // 킬 카운트 결산 텍스트로 출력
        killCountResultText.text = PlayerPrefs.GetInt("Player_KillCount").ToString();
        // 보너스 점수 산출
        bonusScoreText.GetComponent<Text>().text = (PlayerPrefs.GetInt("Player_KillCount") * 150).ToString();

        #endregion
    }
}
