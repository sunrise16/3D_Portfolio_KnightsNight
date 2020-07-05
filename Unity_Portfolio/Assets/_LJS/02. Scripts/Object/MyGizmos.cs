using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    #region 제어값 변수 관련

    // 기즈모의 타입 열거문
    public enum Type { NORMAL, WAYPOINT }
    // 기즈모의 이름
    private const string wayPointFile = "Enemy";
    // 기즈모의 타입 종류값
    public Type type = Type.NORMAL;
    // 기즈모의 색상
    public Color _color = Color.yellow;
    // 기즈모의 반지름
    public float _radius = 0.1f;

    #endregion

    #region 기즈모 출력 함수 

    private void OnDrawGizmos()
    {
        // 노멀 타입일 경우
        if (type == Type.NORMAL)
        {
            // 기즈모 색상 설정
            Gizmos.color = _color;
            // 구체 모양의 기즈모 생성 (인자는 생성 위치, 반지름)
            Gizmos.DrawSphere(transform.position, _radius);
        }
        // 웨이포인트 타입일 경우
        else
        {
            // 기즈모 색상 설정
            Gizmos.color = _color;
            // Enemy 이미지 파일 표시
            Gizmos.DrawIcon(transform.position + Vector3.up * 1.0f, wayPointFile, true);
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }

    #endregion
}
