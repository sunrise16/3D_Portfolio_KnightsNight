using UnityEngine;
using UnityEngine.UI;

public class MPPrice : MonoBehaviour
{
    #region 컴포넌트 변수 관련

    // 마력 구입 가격 텍스트 컴포넌트
    public Text mpPriceText;

    #endregion

    void Start()
    {
        #region 컴포넌트 변수 관련 초기화

        // 마력 구입 가격 텍스트 컴포넌트 할당
        mpPriceText = GetComponent<Text>();

        #endregion
    }

    void Update()
    {
        #region 제어값 변수 갱신

        // 마력 구입 가격 텍스트로 출력
        mpPriceText.text = DataManager.instance.playerMPPrice.ToString();

        #endregion
    }
}
