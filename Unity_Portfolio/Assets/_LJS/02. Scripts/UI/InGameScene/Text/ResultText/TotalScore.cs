using UnityEngine;
using UnityEngine.UI;

public class TotalScore : MonoBehaviour
{
    #region 컴포넌트 변수 관련

    // 총합 점수 텍스트 컴포넌트
    public Text totalScoreText;

    #endregion

    void Start()
    {
        #region 컴포넌트 변수 관련 초기화

        // 총합 점수 텍스트 컴포넌트 할당
        totalScoreText = GetComponent<Text>();

        #endregion
    }

    void Update()
    {
        #region 제어값 변수 갱신

        // 총합 점수 텍스트로 출력
        totalScoreText.text = ((DataManager.instance.playerCurrentKillCount * 50) +
            (DataManager.instance.playerMaxCombo * 100) + (DataManager.instance.playerGold * 10)).ToString();

        #endregion
    }
}
