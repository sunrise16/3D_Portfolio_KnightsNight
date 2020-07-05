using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    #region 제어값 변수 관련

    // 적 몬스터의 현재 HP
    public int enemyCurrentHP;
    // 적 몬스터의 최대 HP
    public int enemyMaxHP;
    // 적 몬스터의 공격력 최소값
    public int enemyAttackMin;
    // 적 몬스터의 공격력 최대값
    public int enemyAttackMax;
    // 적 몬스터의 공격력
    public int enemyAttack;
    // 적 몬스터의 방어력
    public int enemyDefense;
    // 적 몬스터의 이동 속도
    public float enemySpeed;
    // 적 몬스터의 소지 금액 최소값
    public int enemyGoldMin;
    // 적 몬스터의 소지 금액 최대값
    public int enemyGoldMax;
    // 적 몬스터의 소지 금액
    public int enemyGold;

    #endregion

    void Update()
    {
        #region 실시간 제어값 갱신

        // 적 몬스터의 공격력 랜덤 지정
        enemyAttack = Random.Range(enemyAttackMin, enemyAttackMax);

        // 적 몬스터가 드랍하는 골드 랜덤 지정
        enemyGold = Random.Range(enemyGoldMin, enemyGoldMax);

        #endregion
    }
}