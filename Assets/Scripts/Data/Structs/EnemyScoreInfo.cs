using DungeonMaster.Data.Enums;

namespace DungeonMaster.Data.Structs
{
    /// <summary>
    /// 점수 계산에 필요한 적의 정보를 담는 구조체입니다.
    /// </summary>
    public struct EnemyScoreInfo
    {
        public string Name;      // 적의 이름 (로그 용도)
        public EnemyType EnemyType; // 적의 종류
        public int BaseScore;    // 처치 시 기본 점수
    }
}

