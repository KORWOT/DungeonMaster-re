using System.Collections.Generic;
using Data;
using Data.Enums;
using UnityEngine;

namespace Combat.Growth
{
    /// <summary>
    /// 베이스 카드의 강화(동일 카드 합성, 재료 사용 등)를 관리하는 클래스입니다.
    /// </summary>
    public class BaseCardUpgradeManager
    {
        /// <summary>
        /// 동일한 카드를 재료로 사용하여 카드를 강화합니다. (구현 필요)
        /// </summary>
        public UpgradeResult UpgradeWithSameCard(BaseMonsterCard target, BaseMonsterCard material)
        {
            // TODO: 동일 카드 강화 로직 구현.
            // 예: 경험치 대량 획득, 스킬 레벨업 확률 증가, 최대 강화 레벨 확장 등
            Debug.Log($"{target.MonsterId}를 {material.MonsterId}(으)로 강화 (미구현)");
            return new UpgradeResult { IsSuccess = false, Message = "미구현된 기능입니다." };
        }

        /// <summary>
        /// 특수 재화를 사용하여 카드를 강화합니다. (구현 필요)
        /// </summary>
        public UpgradeResult UpgradeWithMaterials(BaseMonsterCard target, object[] materials)
        {
            // TODO: 재료를 사용한 강화 로직 구현.
            Debug.Log($"{target.MonsterId}를 재료로 강화 (미구현)");
            return new UpgradeResult { IsSuccess = false, Message = "미구현된 기능입니다." };
        }
        
        /// <summary>
        /// 다른 카드를 희생하여 특정 스킬의 레벨을 올립니다. (구현 필요)
        /// </summary>
        public UpgradeResult UpgradeSkill(BaseMonsterCard target, int skillIndex, List<BaseMonsterCard> sacrificeCards)
        {
            // TODO: 스킬 강화 로직 구현.
            // 희생 카드의 등급, 레벨에 따라 성공 확률이 달라질 수 있음.
            Debug.Log($"{target.MonsterId}의 스킬 강화 (미구현)");
            return new UpgradeResult { IsSuccess = false, Message = "미구현된 기능입니다." };
        }

        /// <summary>
        /// 현재 성장 등급에 따른 강화 성공 확률을 계산합니다. (구현 필요)
        /// </summary>
        public float CalculateUpgradeChance(GrowthGrade currentGrade)
        {
            // TODO: 등급별 성공 확률을 정의한 설정 파일을 기반으로 계산.
            // 예: S등급은 성공 확률이 매우 낮음.
            return 100f;
        }
    }
}
