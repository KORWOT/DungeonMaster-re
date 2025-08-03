using DungeonMaster.Data;
using DungeonMaster.Data.Enums;
using DungeonMaster.Settings;
using UnityEngine;

namespace DungeonMaster.Systems.Progression
{
    // 강화 결과를 나타내는 클래스. 성공 여부, 결과물 카드 등을 포함할 수 있음.
    public class UpgradeResult 
    {
        public bool IsSuccess;
        public BaseMonsterCardData ResultCard;
        public string Message;
    }

    /// <summary>
    /// 카드의 성장률 관련 로직을 관리하는 클래스입니다.
    /// </summary>
    public class GrowthRateManager
    {
        /// <summary>
        /// 성장률 수치를 기반으로 등급을 결정합니다.
        /// </summary>
        /// <param name="growthRate">성장률 수치</param>
        /// <param name="settings">성장률 등급 설정 파일</param>
        /// <returns>결정된 성장률 등급</returns>
        public GrowthGrade DetermineGrade(float growthRate, GrowthRateSettings settings)
        {
            if (settings == null)
            {
                Debug.LogError("GrowthRateSettings가 제공되지 않았습니다.");
                return GrowthGrade.F;
            }
            return settings.DetermineGrade(growthRate);
        }

        /// <summary>
        /// 카드의 특정 스탯 성장률을 강화합니다. (구현 필요)
        /// </summary>
        public UpgradeResult UpgradeGrowthRate(BaseMonsterCardData card, StatType statType, object[] materials)
        {
            // TODO: 재료를 소모하여 성장률을 강화하는 로직 구현.
            // 강화 확률 계산, 성공/실패 처리 등이 필요.
            Debug.Log($"{card.MonsterId}의 {statType} 성장률 강화 시도 (미구현)");
            return new UpgradeResult { IsSuccess = false, Message = "미구현된 기능입니다." };
        }

        /// <summary>
        /// 성장률 수치가 유효한 범위 내에 있는지 확인합니다. (구현 필요)
        /// </summary>
        public bool IsValidGrowthRate(float rate, GrowthRateSettings settings)
        {
            // TODO: 설정 파일에 정의된 전체 min/max 범위를 벗어나는지 확인하는 로직.
            return true;
        }
    }
}
