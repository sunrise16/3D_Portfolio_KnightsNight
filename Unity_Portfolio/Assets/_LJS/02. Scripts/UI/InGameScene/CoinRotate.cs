using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CoinRotate : MonoBehaviour
{
    #region 컴포넌트 변수 관련

    // 프레임 별 애니메이션 이미지
    public Image image;
    public Sprite[] frameImage;

    #endregion

    #region 제어값 변수 관련

    // 애니메이션 프레임값
    private int frameIndex;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        #region 컴포넌트 변수 관련 초기화

        // 이미지 컴포넌트 가져오기
        image = GetComponent<Image>();

        #endregion

        #region 제어값 변수 관련 초기화

        // 애니메이션 프레임값 초기화
        frameIndex = 0;

        #endregion

        #region 코루틴 함수 실행

        // 이미지 프레임 애니메이션 코루틴
        StartCoroutine(FrameImage());
        
        #endregion
    }

    #region 코루틴 함수

    #region 이미지 프레임 애니메이션

    IEnumerator FrameImage()
    {
        while (true)
        {
            frameIndex++;
            if (frameIndex > 3)
            {
                frameIndex = 0;
            }
            image.sprite = frameImage[frameIndex];

            yield return new WaitForSeconds(0.1f);
        }
    }

    #endregion

    #endregion
}
