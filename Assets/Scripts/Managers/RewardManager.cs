using Core.Logging;
using Data;

namespace Managers
{
    /// <summary>
    /// 전투 종료 후 보상 지급을 처리하는 클래스입니다.
    /// 보상 규칙은 RewardTable ScriptableObject를 통해 주입받습니다.
    /// </summary>
    public class RewardManager
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

        /// <summary>
        /// 전투 승리 시 보상을 계산하고 지급합니다.
        /// </summary>
        /// <param name="playerData">플레이어의 영구 데이터</param>
        public void GrantVictoryRewards(PlayerData playerData)
        {
            if (_rewardTable == null) return;
            
            GameLogger.Log("전투 승리! 보상을 지급합니다.");

            long crystalReward = _rewardTable.BaseVictoryCrystal;
            long coinReward = _rewardTable.BaseVictoryCoin;

            playerData.Crystal += crystalReward;
            playerData.Coin += coinReward;
            
            GameLogger.Log($"보상: 크리스탈 +{crystalReward}, 코인 +{coinReward}");
            
            // TODO: 카드, 아이템 등 추가 보상 지급 로직
        }

        /// <summary>
        /// 전투 패배 시 보상을 계산하고 지급합니다.
        /// </summary>
        /// <param name="playerData">플레이어의 영구 데이터</param>
        /// <param name="reachedWave">도달한 웨이브 번호</param>
        public void GrantDefeatRewards(PlayerData playerData, int reachedWave)
        {
            if (_rewardTable == null) return;

            GameLogger.Log($"전투 패배... (도달 웨이브: {reachedWave})");

            long crystalReward = reachedWave * _rewardTable.DefeatCrystalPerWave;
            long coinReward = reachedWave * _rewardTable.DefeatCoinPerWave;
            
            playerData.Crystal += crystalReward;
            playerData.Coin += coinReward;

            GameLogger.Log($"보상: 크리스탈 +{crystalReward}, 코인 +{coinReward}");
        }
    }
}
