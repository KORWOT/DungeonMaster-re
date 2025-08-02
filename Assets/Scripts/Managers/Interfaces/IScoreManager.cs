using Combat;

namespace Managers.Interfaces
{
    public interface IScoreManager
    {
        int TotalScore { get; }
        void AddScore(EnemyType enemyType);
        void ResetScore();
    }
}
