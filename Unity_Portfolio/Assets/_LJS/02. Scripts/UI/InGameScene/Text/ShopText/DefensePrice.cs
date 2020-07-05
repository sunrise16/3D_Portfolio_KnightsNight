using UnityEngine;
using UnityEngine.UI;

public class DefensePrice : MonoBehaviour
{
    #region 컴포넌트 변수 관련

    // 방어력 구입 가격 텍스트 컴포넌트
    public Text defensePriceText;

    #endregion

    void Start()
    {
        #region 컴포넌트 변수 관련 초기화

        // 방어력 구입 가격 텍스트 컴포넌트 할당
        defensePriceText = GetComponent<Text>();

        #endregion
    }

    void Update()
    {
        #region 제어값 변수 갱신

        // 방어력 구입 가격 텍스트로 출력
        defensePriceText.text = DataManager.instance.playerDefensePrice.ToString();

        #endregion
    }
}
