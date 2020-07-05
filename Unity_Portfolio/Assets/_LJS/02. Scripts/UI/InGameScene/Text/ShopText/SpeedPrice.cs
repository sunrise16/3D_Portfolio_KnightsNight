using UnityEngine;
using UnityEngine.UI;

public class SpeedPrice : MonoBehaviour
{
    #region 컴포넌트 변수 관련

    // 스피드 구입 가격 텍스트 컴포넌트
    public Text speedPriceText;

    #endregion

    void Start()
    {
        #region 컴포넌트 변수 관련 초기화

        // 스피드 구입 가격 텍스트 컴포넌트 할당
        speedPriceText = GetComponent<Text>();

        #endregion
    }

    void Update()
    {
        #region 제어값 변수 갱신

        // 스피드 구입 가격 텍스트로 출력
        speedPriceText.text = DataManager.instance.playerSpeedPrice.ToString();

        #endregion
    }
}
