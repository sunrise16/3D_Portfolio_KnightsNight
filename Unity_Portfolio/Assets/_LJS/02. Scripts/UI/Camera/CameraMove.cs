using UnityEngine;

public class CameraMove : MonoBehaviour
{
    #region 컴포넌트 변수 관련

    // 타겟으로 잡을 오브젝트
    private GameObject targetObject;
    // 카메라가 위치할 지점
    public GameObject cameraPoint;

    #endregion

    #region 제어값 변수 관련

    // 카메라 회전 속도
    public float cameraRotateSpeed = 150.0f;
    // 회전 각도 제어 변수
    private float cameraAngleX, cameraAngleY;

    #endregion

    void Start()
    {
        #region 컴포넌트 변수 관련 초기화

        // 타겟 오브젝트 지정 (플레이어)
        targetObject = GameObject.Find("Player");

        #endregion
    }

    void Update()
    {
        #region 제어값 변수 갱신

        // 지정한 포인트에 카메라 고정시키기
        if (GameManager.instance.isGameOver == true && GameManager.instance.gameOverDelay >= 0.7f)
        {
            // 카메라 위치 조절
            transform.position = new Vector3(cameraPoint.transform.position.x, cameraPoint.transform.position.y - 2, cameraPoint.transform.position.z + 3);
            
            // 타겟을 향해 바라보기
            transform.LookAt(targetObject.transform.position);
        }
        else
        {
            transform.position = cameraPoint.transform.position;
        }

        #endregion
    }

    void LateUpdate()
    {
        #region 지속적으로 실행할 함수 실행

        if (GameManager.instance.isGameOver == false && GameObject.Find("Player").GetComponent<PlayerFSM>().playerStop == false)
        {
            // 카메라 회전 함수
            Rotate();
        }

        #endregion
    }

    #region 카메라 회전 함수

    void Rotate()
    {
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");

        // Vector3 dir = new Vector3(v, -h, 0);
        // transform.Rotate(dir * speed * Time.deltaTime);

        // 유니티 엔진에서 제공해주는 함수를 사용함에 있어서 Translate 함수는 사용하는데 큰 불편함이 없음
        // 그러나 Rotate 함수는 직접 제어하기가 힘듬 (두 개 이상의 회전축이 겹치게 되는 짐벌락 현상이 발생)
        // Inspector 창의 로테이션 값은 우리가 보기 편하게 오일러 각도로 표시되지만 내부적으로는 Quaternion 값으로 회전 처리가 되고 있음
        // Quaternion을 사용하는 이유는 짐벌락 현상을 방지할 수 있기 때문
        // 회전을 직접 제어할 때는 Rotate 함수를 사용하지 않고 Transform의 Euler Angle을 사용하면 됨

        // eulerAngles를 사용하면 카메라가 고정되었다 풀렸다 하는 문제가 있음
        // 직접 회전각도를 제한해서 처리하면 됨
        // 그러나 여기에도 문제가 있는데 유니티는 내부적으로 음수의 각도는 360도를 더해서 처리됨
        // 즉 자신이 만든 각도를 가지고 계산을 처리해야 함
        // transform.eulerAngles += dir * speed * Time.deltaTime;
        // Vector3 angle = transform.eulerAngles;
        // angle += dir * speed * Time.deltaTime;
        // if (angle.x > 60) angle.x = 60;
        // if (angle.x < -60) angle.x = -60;
        // transform.eulerAngles = angle;

        cameraAngleX += h * cameraRotateSpeed * Time.deltaTime;
        cameraAngleY += v * cameraRotateSpeed * Time.deltaTime;
        // cameraAngleY = Mathf.Clamp(cameraAngleY, -20, -20);
        cameraAngleY = Mathf.Clamp(cameraAngleY, -30, 30);
        transform.eulerAngles = new Vector3(-cameraAngleY, cameraAngleX, 0);
    }

    #endregion
}