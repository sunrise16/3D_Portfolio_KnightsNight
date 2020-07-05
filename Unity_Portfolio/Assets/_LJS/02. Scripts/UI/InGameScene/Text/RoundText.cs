using UnityEngine;
using UnityEngine.UI;

public class RoundText : MonoBehaviour
{
    #region 컴포넌트 변수 관련

    // 현재 라운드 텍스트 컴포넌트
    public Text roundText;

    #endregion

    void Start()
    {
        #region 컴포넌트 변수 관련 초기화

        // 현재 라운드 텍스트 컴포넌트 가져오기
        roundText = GetComponent<Text>();

        #endregion
    }

    void Update()
    {
        #region 제어값 변수 갱신

        // 현재 라운드 텍스트로 출력
        roundText.text = GameManager.instance.currentRound.ToString();

        #endregion
    }
}
