using UnityEngine;
using UnityEngine.UI;

public class GoldText : MonoBehaviour
{
    #region 컴포넌트 변수 관련

    // 현재 소지 금액 텍스트 컴포넌트
    public Text goldText;

    #endregion

    void Start()
    {
        #region 컴포넌트 변수 관련 초기화

        // 현재 소지 금액 텍스트 컴포넌트 할당
        goldText = GetComponent<Text>();

        #endregion
    }

    void Update()
    {
        #region 제어값 변수 갱신

        // 현재 소지 금액 텍스트로 출력
        goldText.text = DataManager.instance.playerGold.ToString();

        #endregion
    }
}