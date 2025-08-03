using DungeonMaster.Core.Interfaces;
using DungeonMaster.Core.Logging;
using DungeonMaster.Data;
using DungeonMaster.Data.Enums;
using DungeonMaster.Gameplay.Actors;

namespace DungeonMaster.Systems.Rewards
{
    /// <summary>
    /// 게임 내 점수를 계산하고 관리하는 클래스입니다.
    /// </summary>
    public class ScoreManager : IScoreManager
    {
        public int TotalScore { get; private set; }

        private readonly RewardTable _rewardTable;
        private readonly int _difficultyLevel;

        /// <summary>
        /// ScoreManager를 초기화합니다.
        /// </summary>
        /// <param name="rewardTable">점수 계산에 사용할 보상 테이블</param>
        /// <param name="difficultyLevel">현재 게임의 난이도</param>
        public ScoreManager(RewardTable rewardTable, int difficultyLevel = 1)
        {
            _rewardTable = rewardTable;
            _difficultyLevel = difficultyLevel > 0 ? difficultyLevel : 1;
            TotalScore = 0;

            if (_rewardTable == null)
            {
                GameLogger.LogError("RewardTable이 주입되지 않았습니다! 점수 계산이 제대로 동작하지 않을 수 있습니다.");
            }
        }
        
        public void AddScore(EnemyType enemyType)
        {
             // 이 메서드는 외부(GameFlowManager)에서 EnemyInfo를 받아 처리하도록 변경될 것입니다.
             // 현재는 인터페이스와의 호환성을 위해 시그니처만 유지합니다.
            throw new System.NotImplementedException("AddScore(EnemyInfo)를 사용해주세요.");
        }


        /// <summary>
        /// 처치한 적의 정보를 바탕으로 점수를 계산하고 총 점수에 더합니다.
        /// </summary>
        /// <param name="enemyInfo">처치한 적의 EnemyInfo 컴포넌트</param>
        public void AddScore(EnemyInfo enemyInfo)
        {
            if (enemyInfo == null || _rewardTable == null) return;

            float score = enemyInfo.BaseScore;

            // 1. 적 타입에 따른 배율 적용
            switch (enemyInfo.EnemyType)
            {
                case EnemyType.Elite:
                    score *= _rewardTable.EliteScoreMultiplier;
                    break;
                case EnemyType.Boss:
                    score *= _rewardTable.BossScoreMultiplier;
                    break;
            }

            // 2. 난이도에 따른 배율 적용 (난이도 1은 배율 없음)
            if (_difficultyLevel > 1)
            {
                score *= UnityEngine.Mathf.Pow(_rewardTable.DifficultyMultiplier, _difficultyLevel - 1);
            }
            
            // TODO: 적의 강함(레벨 등)에 따른 추가 점수 로직 구현
            
            int finalScore = UnityEngine.Mathf.RoundToInt(score);
            TotalScore += finalScore;
            
            GameLogger.Log($"적 처치! {enemyInfo.name} ({enemyInfo.EnemyType}) | 획득 점수: {finalScore} | 총 점수: {TotalScore}");
        }

        /// <summary>
        /// 현재 총 점수를 초기화합니다.
        /// </summary>
        public void ResetScore()
        {
            TotalScore = 0;
            GameLogger.Log("점수가 초기화되었습니다.");
        }
    }
}
