using UnityEngine;

public class NPCMark : MonoBehaviour
{
    #region 컴포넌트 변수 관련

    // 타겟으로 잡을 오브젝트
    private GameObject targetObject;
    // 부모가 될 Canvas 객체
    private Canvas uiCanvas;
    // 상호 작용 아이콘 오브젝트를 저장할 변수
    private GameObject interactionIcon;
    public GameObject interactionIconPrefab;

    #endregion

    #region 제어값 변수 관련

    // NPC와 타겟(플레이어) 오브젝트와의 거리값
    private float distance;
    // 상호 작용 아이콘의 위치를 보정할 오프셋
    private Vector3 interactionIconOffset;
    // 상호 작용 아이콘 출력 여부 판별값
    public bool iconActive;

    #endregion

    void Start()
    {
        #region 컴포넌트 변수 관련 초기화

        // 타겟 오브젝트 지정 (플레이어)
        targetObject = GameObject.Find("Player");

        #endregion

        #region 제어값 변수 관련 초기화

        // NPC와 타겟(플레이어) 오브젝트와의 거리값 초기화
        distance = 0.0f;
        // 상호 작용 아이콘 오프셋 초기화
        interactionIconOffset = new Vector3(0, 3.0f, 0);
        // 상호 작용 아이콘 출력 여부 판별값 초기화
        iconActive = false;

        #endregion

        #region 일반 함수 실행

        // NPC 상호 작용 아이콘 세팅
        SetInteractionIcon();

        #endregion
    }

    void Update()
    {
        #region 실시간 변수값 갱신

        // NPC와 타겟(플레이어) 오브젝트와의 거리값 갱신
        distance = Vector3.Distance(transform.position, targetObject.transform.position);

        #endregion

        #region NPC 상호 작용 아이콘 활성, 비활성화

        if (Mathf.Abs(distance) < 4.0f)
        {
            interactionIcon.SetActive(true);
            iconActive = true;
        }
        else
        {
            interactionIcon.SetActive(false);
            iconActive = false;
        }

        #endregion
    }

    #region NPC 상호 작용 아이콘 세팅

    void SetInteractionIcon()
    {
        uiCanvas = GameObject.Find("UI Canvas").GetComponent<Canvas>();
        // UI Canvas 하위로 HP 게이지 생성
        interactionIcon = Instantiate(interactionIconPrefab, uiCanvas.transform);

        // 발견 아이콘이 따라가야 할 대상과 오프셋 값 설정
        var _findIcon = interactionIcon.GetComponent<ImageOutput>();
        _findIcon.targetTr = this.gameObject.transform;
        _findIcon.offset = interactionIconOffset;

        interactionIcon.SetActive(false);
    }

    #endregion
}
