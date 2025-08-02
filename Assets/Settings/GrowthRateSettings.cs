using Data.Enums;
using UnityEngine;

namespace Settings
{
    /// <summary>
    /// 성장률 등급(F~S)별 실제 성장률 수치 범위를 정의하는 ScriptableObject입니다.
    /// </summary>
    [CreateAssetMenu(fileName = "GrowthRateSettings", menuName = "Settings/Growth Rate Settings")]
    public class GrowthRateSettings : ScriptableObject
    {
        [System.Serializable]
        public struct GrowthGradeSetting
        {
            public GrowthGrade Grade;
            [Tooltip("이 등급의 최소 성장률 수치 (포함)")]
            public float MinRate;
            [Tooltip("이 등급의 최대 성장률 수치 (포함)")]
            public float MaxRate;
        }

        public GrowthGradeSetting[] GrowthSettingsList;

        /// <summary>
        /// 주어진 성장률 수치가 어떤 등급에 해당하는지 결정합니다.
        /// </summary>
        public GrowthGrade DetermineGrade(float rate)
        {
            foreach (var setting in GrowthSettingsList)
            {
                if (rate >= setting.MinRate && rate <= setting.MaxRate)
                {
                    return setting.Grade;
                }
            }
            Debug.LogError($"주어진 성장률({rate})에 해당하는 등급을 찾을 수 없습니다.");
            return GrowthGrade.F;
        }
    }
}
