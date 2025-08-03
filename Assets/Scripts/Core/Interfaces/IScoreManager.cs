using DungeonMaster.Data.Enums;

namespace DungeonMaster.Core.Interfaces
{
    public interface IScoreManager
    {
        int TotalScore { get; }
        void AddScore(EnemyType enemyType);
        void ResetScore();
    }
}
