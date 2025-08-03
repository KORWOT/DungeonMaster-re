using System.Collections.Generic;
using UnityEngine;

namespace DungeonMaster.Data
{
    /// <summary>
    /// 단일 보상 항목을 정의하는 구조체입니다 (예: 재화, 경험치).
    /// </summary>
    [System.Serializable]
    public struct RewardItem
    {
        // TODO: 재화, 경험치, 아이템 등을 구분할 수 있는 RewardType enum 추가 필요
        [Tooltip("보상의 종류 (예: Crystal, Coin, Exp)")]
        public string Type;
        [Tooltip("보상의 양")]
        public long Amount;
    }

    /// <summary>
    /// 특정 점수 구간에 도달했을 때 지급되는 보상 등급(Tier)을 정의합니다.
    /// </summary>
    [System.Serializable]
    public class RewardTier
    {
        [Tooltip("이 등급의 보상을 받기 위해 달성해야 하는 최소 점수")]
        public int ScoreThreshold;
        [Tooltip("달성 시 지급되는 보상 목록")]
        public List<RewardItem> Rewards;
    }
    
    /// <summary>
    /// 획득한 점수에 따른 보상 규칙을 정의하는 ScriptableObject입니다.
    /// </summary>
    [CreateAssetMenu(fileName = "New RewardTableData", menuName = "Data/Reward Table")]
    public class RewardTableData : ScriptableObject
    {
        [Header("점수 기반 보상 등급")]
        [Tooltip("점수 순서대로 정렬되어야 합니다 (낮은 점수 -> 높은 점수).")]
        public List<RewardTier> Tiers;

        [Header("점수 계산 계수")]
        [Tooltip("난이도 1당 점수 배율 (예: 1.1 = 난이도마다 10%씩 점수 증가)")]
        public float DifficultyMultiplier = 1.1f;
        
        [Tooltip("엘리트 몬스터의 점수 배율")]
        public float EliteScoreMultiplier = 2.0f;
        
        [Tooltip("보스 몬스터의 점수 배율")]
        public float BossScoreMultiplier = 5.0f;

        /// <summary>
        /// 주어진 총 점수에 해당하는 가장 높은 보상 등급(Tier)을 반환합니다.
        /// </summary>
        /// <param name="totalScore">플레이어가 획득한 총 점수</param>
        /// <returns>해당하는 보상 등급. 없으면 null.</returns>
        public RewardTier GetTierForScore(int totalScore)
        {
            // 점수가 높은 등급부터 역순으로 검사하여, 조건을 만족하는 첫 번째 등급을 반환합니다.
            for (int i = Tiers.Count - 1; i >= 0; i--)
            {
                if (totalScore >= Tiers[i].ScoreThreshold)
                {
                    return Tiers[i];
                }
            }
            return null; // 아무 등급도 달성하지 못한 경우
        }
    }
}
