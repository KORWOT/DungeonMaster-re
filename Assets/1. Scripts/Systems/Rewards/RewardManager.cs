using DungeonMaster.Core.Interfaces;
using DungeonMaster.Core.Logging;
using DungeonMaster.Data;

namespace DungeonMaster.Systems.Rewards
{
    /// <summary>
    /// 전투 종료 후 보상 지급을 처리하는 클래스입니다.
    /// 보상 규칙은 RewardTable ScriptableObject를 통해 주입받습니다.
    /// </summary>
    public class RewardManager : IRewardManager
    {
        private readonly RewardTable _rewardTable;

        /// <summary>
        /// RewardManager를 초기화합니다.
        /// </summary>
        /// <param name="rewardTable">사용할 보상 테이블 데이터</param>
        public RewardManager(RewardTable rewardTable)
        {
            _rewardTable = rewardTable;
            if (_rewardTable == null)
            {
                GameLogger.LogError("RewardTable이 주입되지 않았습니다! 보상 시스템이 제대로 동작하지 않을 수 있습니다.");
            }
        }
        
        public void DistributeRewards(int totalScore, PlayerData playerData)
        {
            if (_rewardTable == null || playerData == null) return;

            GameLogger.Log($"게임 종료! 최종 점수: {totalScore}. 보상을 지급합니다.");

            RewardTier finalTier = _rewardTable.GetTierForScore(totalScore);

            if (finalTier == null)
            {
                GameLogger.Log("달성한 보상 등급이 없어 지급되는 보상이 없습니다.");
                return;
            }

            GameLogger.Log($"달성 보상 등급: {finalTier.ScoreThreshold}점 등급");
            foreach (var rewardItem in finalTier.Rewards)
            {
                // TODO: RewardItem의 Type에 따라 재화, 경험치, 아이템 등을 구분하여 지급하는 로직 구현
                if (rewardItem.Type.Equals("Crystal", System.StringComparison.OrdinalIgnoreCase))
                {
                    playerData.Crystal += rewardItem.Amount;
                    GameLogger.Log($"크리스탈 +{rewardItem.Amount} (총: {playerData.Crystal})");
                }
                else if (rewardItem.Type.Equals("Coin", System.StringComparison.OrdinalIgnoreCase))
                {
                    playerData.Coin += rewardItem.Amount;
                    GameLogger.Log($"코인 +{rewardItem.Amount} (총: {playerData.Coin})");
                }
            }
        }
    }
}
