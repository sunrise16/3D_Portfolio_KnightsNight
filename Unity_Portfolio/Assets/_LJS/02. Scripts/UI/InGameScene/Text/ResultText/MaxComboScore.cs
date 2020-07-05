using UnityEngine;
using UnityEngine.UI;

public class MaxComboScore : MonoBehaviour
{
    #region 컴포넌트 변수 관련

    // 현재 최대 콤보수 점수 텍스트 컴포넌트
    public Text maxComboScoreText;

    #endregion

    void Start()
    {
        #region 컴포넌트 변수 관련 초기화

        // 현재 최대 콤보수 점수 텍스트 컴포넌트 할당
        maxComboScoreText = GetComponent<Text>();

        #endregion
    }

    void Update()
    {
        #region 제어값 변수 갱신

        // 현재 최대 콤보수 점수 텍스트로 출력
        maxComboScoreText.text = (DataManager.instance.playerMaxCombo * 100).ToString();

        #endregion
    }
}
