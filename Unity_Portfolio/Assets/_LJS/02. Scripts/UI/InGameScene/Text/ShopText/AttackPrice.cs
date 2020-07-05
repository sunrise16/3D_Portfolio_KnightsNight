using UnityEngine;
using UnityEngine.UI;

public class AttackPrice : MonoBehaviour
{
    #region 컴포넌트 변수 관련

    // 공격력 구입 가격 텍스트 컴포넌트
    public Text attackPriceText;

    #endregion

    void Start()
    {
        #region 컴포넌트 변수 관련 초기화

        // 공격력 구입 가격 텍스트 컴포넌트 할당
        attackPriceText = GetComponent<Text>();

        #endregion
    }

    void Update()
    {
        #region 제어값 변수 갱신

        // 공격력 구입 가격 텍스트로 출력
        attackPriceText.text = DataManager.instance.playerAttackPrice.ToString();

        #endregion
    }
}
