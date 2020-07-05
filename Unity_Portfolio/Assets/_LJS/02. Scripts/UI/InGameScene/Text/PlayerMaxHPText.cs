using UnityEngine;
using UnityEngine.UI;

public class PlayerMaxHPText : MonoBehaviour
{
    #region 컴포넌트 변수 관련

    // 현재 최대 체력 수치 텍스트 컴포넌트
    public Text playerMaxHPText;

    #endregion

    void Start()
    {
        #region 컴포넌트 변수 관련 초기화

        // 현재 최대 체력 수치 텍스트 컴포넌트 할당
        playerMaxHPText = GetComponent<Text>();

        #endregion
    }

    void Update()
    {
        #region 제어값 변수 갱신

        // 현재 최대 체력 수치 텍스트로 출력
        playerMaxHPText.text = DataManager.instance.playerMaxHP.ToString();

        #endregion
    }
}
