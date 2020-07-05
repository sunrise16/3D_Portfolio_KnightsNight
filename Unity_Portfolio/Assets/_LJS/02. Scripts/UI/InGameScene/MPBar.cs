using UnityEngine;
using UnityEngine.UI;

public class MPBar : MonoBehaviour
{
    #region 컴포넌트 변수 관련

    // 슬라이더 컴포넌트
    private Slider slider;

    #endregion

    void Start()
    {
        #region 컴포넌트 변수 관련 초기화

        // 슬라이더 컴포넌트 할당
        slider = GetComponent<Slider>();

        #endregion
    }

    void Update()
    {
        #region 제어값 변수 갱신

        // 플레이어의 마력에 비례하여 슬라이더 밸류값 설정
        slider.value = (float)DataManager.instance.playerCurrentMP / DataManager.instance.playerMaxMP;

        #endregion
    }
}