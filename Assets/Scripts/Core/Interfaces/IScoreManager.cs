using DungeonMaster.Data.Structs;

namespace DungeonMaster.Core.Interfaces
{
    public interface IScoreManager
    {
        int TotalScore { get; }
        void AddScore(EnemyScoreInfo enemyInfo);
        void ResetScore();
    }
}
