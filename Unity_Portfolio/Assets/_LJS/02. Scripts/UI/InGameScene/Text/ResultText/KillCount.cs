using UnityEngine;
using UnityEngine.UI;

public class KillCount : MonoBehaviour
{
    #region 컴포넌트 변수 관련

    // 현재 최대 킬 수 텍스트 컴포넌트
    public Text killCountText;

    #endregion

    void Start()
    {
        #region 컴포넌트 변수 관련 초기화

        // 현재 최대 킬 수 텍스트 컴포넌트 할당
        killCountText = GetComponent<Text>();

        #endregion
    }

    void Update()
    {
        #region 제어값 변수 갱신

        // 현재 최대 킬 수 텍스트로 출력
        killCountText.text = DataManager.instance.playerCurrentKillCount.ToString();

        #endregion
    }
}
