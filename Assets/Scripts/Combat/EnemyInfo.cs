using Data.Enums;
using UnityEngine;

namespace Combat
{
    /// <summary>
    /// 적 유닛의 기본 정보(타입, 처치 점수 등)를 담는 컴포넌트입니다.
    /// 이 컴포넌트는 모든 적 프리팹에 부착되어야 합니다.
    /// </summary>
    public class EnemyInfo : MonoBehaviour
    {
        [Header("적 정보")]
        [Tooltip("적의 종류 (Normal, Elite, Boss)")]
        [SerializeField] private EnemyType enemyType = EnemyType.Normal;
        
        [Tooltip("이 적을 처치했을 때 획득하는 기본 점수")]
        [SerializeField] private int baseScore = 10;
        
        public EnemyType EnemyType => enemyType;
        public int BaseScore => baseScore;
    }
}
