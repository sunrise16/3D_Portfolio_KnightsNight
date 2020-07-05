using UnityEngine;
using UnityEngine.UI;

public class TimeText : MonoBehaviour
{
    #region 컴포넌트 변수 관련

    // 현재 타임 텍스트 컴포넌트
    public Text timeText;

    #endregion

    void Start()
    {
        #region 컴포넌트 변수 관련 초기화

        // 현재 타임 텍스트 컴포넌트 가져오기
        timeText = GetComponent<Text>();

        #endregion
    }

    void Update()
    {
        #region 제어값 변수 갱신

        // 현재 타임 텍스트로 출력
        timeText.text = GameManager.instance.timeRemaining.ToString("000");

        #endregion
    }
}
