using DungeonMaster.Data.Enums;
using Settings;
using UnityEngine;

namespace DungeonMaster.Gameplay.Combat.Calculators
{
    /// <summary>
    /// 속성 관련 계산을 담당하는 static 클래스입니다.
    /// 모든 계산은 외부에서 전달된 설정 파일에 의존합니다.
    /// </summary>
    public static class ElementalCalculator
    {
        /// <summary>
        /// 두 속성 간의 데미지 배율을 계산합니다.
        /// </summary>
        /// <param name="attacker">공격자 속성</param>
        /// <param name="defender">방어자 속성</param>
        /// <param name="settings">속성 상성 관계가 정의된 설정 파일</param>
        /// <returns>데미지 배율</returns>
        public static float GetDamageMultiplier(ElementType attacker, ElementType defender, ElementalSettings settings)
        {
            if (settings == null)
            {
                Debug.LogError("ElementalSettings가 제공되지 않았습니다.");
                return 1f;
            }
            return settings.GetDamageMultiplier(attacker, defender);
        }

        /// <summary>
        /// 공격자가 방어자에게 약점(유리한) 공격을 하는지 확인합니다.
        /// </summary>
        /// <param name="attacker">공격자 속성</param>
        /// <param name="defender">방어자 속성</param>
        /// <param name="settings">속성 상성 관계가 정의된 설정 파일</param>
        /// <returns>데미지 배율이 1.0f보다 크면 true</returns>
        public static bool IsWeakAgainst(ElementType attacker, ElementType defender, ElementalSettings settings)
        {
            return GetDamageMultiplier(attacker, defender, settings) > 1.0f;
        }

        /// <summary>
        /// 공격자가 방어자에게 강점(불리한) 공격을 하는지 확인합니다.
        /// </summary>
        /// <param name="attacker">공격자 속성</param>
        /// <param name="defender">방어자 속성</param>
        /// <param name="settings">속성 상성 관계가 정의된 설정 파일</param>
        /// <returns>데미지 배율이 1.0f보다 작으면 true</returns>
        public static bool IsStrongAgainst(ElementType attacker, ElementType defender, ElementalSettings settings)
        {
            return GetDamageMultiplier(attacker, defender, settings) < 1.0f;
        }
    }
}
