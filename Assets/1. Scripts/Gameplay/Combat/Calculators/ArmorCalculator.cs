using DungeonMaster.Data.Enums;
using Settings;
using UnityEngine;

namespace DungeonMaster.Gameplay.Combat.Calculators
{
    /// <summary>
    /// 방어구 타입 관련 계산을 담당하는 static 클래스입니다.
    /// </summary>
    public static class ArmorCalculator
    {
        /// <summary>
        /// 방어구 타입에 따른 보호율(%)을 가져옵니다.
        /// </summary>
        /// <param name="armorType">방어구 타입</param>
        /// <param name="settings">방어구 설정 파일</param>
        /// <returns>보호율 (0~100)</returns>
        public static float GetProtectionRate(ArmorType armorType, ArmorSettings settings)
        {
            if (settings == null)
            {
                Debug.LogError("ArmorSettings가 제공되지 않았습니다.");
                return 0f;
            }
            return settings.GetSetting(armorType).ProtectionRate;
        }

        /// <summary>
        /// 방어구 타입에 따른 공격 속도 변화율(%)을 가져옵니다.
        /// </summary>
        /// <param name="armorType">방어구 타입</param>
        /// <param name="settings">방어구 설정 파일</param>
        /// <returns>공격 속도 변화율 (%)</returns>
        public static float GetAttackSpeedModifier(ArmorType armorType, ArmorSettings settings)
        {
            if (settings == null)
            {
                Debug.LogError("ArmorSettings가 제공되지 않았습니다.");
                return 0f;
            }
            return settings.GetSetting(armorType).AttackSpeedModifier;
        }

        /// <summary>
        /// 보호율을 적용하여 최종 데미지를 계산합니다.
        /// </summary>
        /// <param name="damage">원래 데미지</param>
        /// <param name="protectionRate">보호율 (%)</param>
        /// <returns>감소된 최종 데미지</returns>
        public static float CalculateDamageReduction(float damage, float protectionRate)
        {
            float reduction = Mathf.Clamp(protectionRate, 0f, 100f) / 100f;
            return damage * (1 - reduction);
        }
    }
}
