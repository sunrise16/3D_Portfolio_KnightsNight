using UnityEngine;
using UnityEngine.UI;

public class KillText : MonoBehaviour
{
    #region 컴포넌트 변수 관련

    // 현재 적 몬스터 킬 수 텍스트 컴포넌트
    public Text killText;

    #endregion

    void Start()
    {
        #region 컴포넌트 변수 관련 초기화

        // 현재 적 몬스터 킬 수 텍스트 컴포넌트 할당
        killText = GetComponent<Text>();

        #endregion
    }

    void Update()
    {
        #region 제어값 변수 갱신

        // 현재 적 몬스터 킬 수 텍스트로 출력
        killText.text = DataManager.instance.playerMaxKillCount.ToString();

        #endregion
    }
}
