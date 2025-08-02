using Data.Enums;
using Settings;
using UnityEngine;

namespace Combat.Calculators
{
    /// <summary>
    /// 치명타 관련 계산을 담당하는 static 클래스입니다.
    /// </summary>
    public static class CriticalCalculator
    {
        /// <summary>
        /// 치명타 등급을 실제 치명타 확률(%)로 변환합니다.
        /// </summary>
        /// <param name="grade">치명타 등급</param>
        /// <param name="settings">치명타 설정 파일</param>
        /// <returns>치명타 확률 (%)</returns>
        public static float GetCriticalRate(GrowthGrade grade, CriticalSettings settings)
        {
            if (settings == null)
            {
                Debug.LogError("CriticalSettings가 제공되지 않았습니다.");
                return 0f;
            }
            // TODO: 등급에 따른 정확한 값 반환 로직 필요 (예: 설정의 Min/Max 사이의 값)
            // 현재는 임시로 MinValue를 반환합니다.
            foreach (var setting in settings.CriticalRateSettings)
            {
                if (setting.Grade == grade) return setting.MinValue;
            }
            return 0f;
        }

        /// <summary>
        /// 치명타 등급을 실제 치명타 피해량(%)으로 변환합니다.
        /// </summary>
        /// <param name="grade">치명타 등급</param>
        /// <param name="settings">치명타 설정 파일</param>
        /// <returns>치명타 피해량 (%)</returns>
        public static float GetCriticalDamage(GrowthGrade grade, CriticalSettings settings)
        {
            if (settings == null)
            {
                Debug.LogError("CriticalSettings가 제공되지 않았습니다.");
                return 0f;
            }
            // TODO: 등급에 따른 정확한 값 반환 로직 필요
            // 현재는 임시로 MinValue를 반환합니다.
            foreach (var setting in settings.CriticalDamageSettings)
            {
                if (setting.Grade == grade) return setting.MinValue;
            }
            return 0f;
        }

        /// <summary>
        /// 주어진 확률로 치명타가 발생했는지 여부를 결정합니다.
        /// </summary>
        /// <param name="criticalRate">치명타 확률 (%)</param>
        /// <returns>치명타 발생 시 true</returns>
        public static bool IsCriticalHit(float criticalRate)
        {
            float rate = Mathf.Clamp(criticalRate, 0f, 100f);
            return Random.Range(0f, 100f) < rate;
        }

        /// <summary>
        /// 치명타 피해량을 적용하여 최종 데미지를 계산합니다.
        /// </summary>
        /// <param name="baseDamage">기본 데미지</param>
        /// <param name="criticalDamage">치명타 피해량 (%)</param>
        /// <returns>치명타가 적용된 최종 데미지</returns>
        public static float ApplyCriticalDamage(float baseDamage, float criticalDamage)
        {
            float multiplier = 1 + (criticalDamage / 100f);
            return baseDamage * multiplier;
        }
    }
}
