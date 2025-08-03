using DungeonMaster.Data;

namespace DungeonMaster.Core.Interfaces
{
    public interface IRewardManager
    {
        void DistributeRewards(int totalScore, PlayerData playerData);
    }
}
