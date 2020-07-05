using UnityEngine;
using UnityEngine.UI;

public class GoldRemainResult : MonoBehaviour
{
    #region 컴포넌트 변수 관련

    // 남은 골드 결산 텍스트 컴포넌트
    public Text goldRemainResultText;
    // 보너스 점수를 담을 텍스트
    public GameObject bonusScoreText;

    #endregion

    void Start()
    {
        #region 컴포넌트 변수 관련 초기화

        // 남은 골드 결산 텍스트 컴포넌트 할당
        goldRemainResultText = GetComponent<Text>();

        #endregion
    }

    void Update()
    {
        #region 제어값 변수 갱신

        // 남은 골드 결산 텍스트로 출력
        goldRemainResultText.text = PlayerPrefs.GetInt("Player_GoldRemain").ToString();
        // 보너스 점수 산출
        bonusScoreText.GetComponent<Text>().text = (PlayerPrefs.GetInt("Player_GoldRemain") * 100).ToString();

        #endregion
    }
}
