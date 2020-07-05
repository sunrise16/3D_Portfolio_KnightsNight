using UnityEngine;
using UnityEngine.UI;

public class PlayerSpeedText : MonoBehaviour
{
    #region 컴포넌트 변수 관련

    // 현재 플레이어 스피드 텍스트 컴포넌트
    public Text playerSpeedText;

    #endregion

    void Start()
    {
        #region 컴포넌트 변수 관련 초기화

        // 현재 플레이어 스피드 텍스트 컴포넌트 가져오기
        playerSpeedText = GetComponent<Text>();

        #endregion
    }

    void Update()
    {
        #region 제어값 변수 갱신

        // 현재 플레이어 스피드 텍스트로 출력
        playerSpeedText.text = DataManager.instance.playerSpeed.ToString();

        #endregion
    }
}
