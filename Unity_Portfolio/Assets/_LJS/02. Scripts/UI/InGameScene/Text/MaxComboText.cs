using UnityEngine;
using UnityEngine.UI;

public class MaxComboText : MonoBehaviour
{
    #region 컴포넌트 변수 관련

    // 현재 최대 콤보수 텍스트 컴포넌트
    public Text maxComboText;

    #endregion

    void Start()
    {
        #region 컴포넌트 변수 관련 초기화

        // 현재 최대 콤보수 텍스트 컴포넌트 할당
        maxComboText = GetComponent<Text>();

        #endregion
    }

    void Update()
    {
        #region 제어값 변수 갱신

        // 현재 최대 콤보수 텍스트로 출력
        maxComboText.text = DataManager.instance.playerMaxCombo.ToString();

        #endregion
    }
}
