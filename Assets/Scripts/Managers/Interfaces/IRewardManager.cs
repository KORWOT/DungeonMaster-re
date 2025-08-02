using Data;

namespace Managers.Interfaces
{
    public interface IRewardManager
    {
        void DistributeRewards(int totalScore, PlayerData playerData);
    }
}
