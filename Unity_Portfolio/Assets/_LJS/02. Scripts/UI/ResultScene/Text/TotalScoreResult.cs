using UnityEngine;
using UnityEngine.UI;

public class TotalScoreResult : MonoBehaviour
{
    #region 컴포넌트 변수 관련

    // 최종 점수 결산 텍스트 컴포넌트
    public Text totalScoreResultText;

    #endregion

    void Start()
    {
        #region 컴포넌트 변수 관련 초기화

        // 최종 점수 결산 텍스트 컴포넌트 할당
        totalScoreResultText = GetComponent<Text>();

        #endregion
    }

    void Update()
    {
        #region 제어값 변수 갱신

        // 최종 점수 결산 텍스트로 출력
        totalScoreResultText.text = (PlayerPrefs.GetInt("Player_TotalScore") + (PlayerPrefs.GetInt("Player_KillCount") * 150) +
            (PlayerPrefs.GetInt("Player_MaxCombo") * 200) + (PlayerPrefs.GetInt("Player_GoldRemain") * 100)).ToString();

        #endregion
    }
}
