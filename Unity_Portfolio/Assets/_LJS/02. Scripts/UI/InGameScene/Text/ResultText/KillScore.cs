using UnityEngine;
using UnityEngine.UI;

public class KillScore : MonoBehaviour
{
    #region 컴포넌트 변수 관련

    // 현재 최대 킬 수 점수 텍스트 컴포넌트
    public Text killScoreText;

    #endregion

    void Start()
    {
        #region 컴포넌트 변수 관련 초기화

        // 현재 최대 킬 수 점수 텍스트 컴포넌트 할당
        killScoreText = GetComponent<Text>();

        #endregion
    }

    void Update()
    {
        #region 제어값 변수 갱신

        // 현재 최대 킬 수 점수 텍스트로 출력
        killScoreText.text = (DataManager.instance.playerCurrentKillCount * 50).ToString();

        #endregion
    }
}
