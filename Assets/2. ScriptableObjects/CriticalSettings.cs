using Data.Enums;
using UnityEngine;

namespace Settings
{
    /// <summary>
    /// 치명타 등급(F~S)별 실제 치명타 관련 수치 범위를 정의하는 ScriptableObject입니다.
    /// </summary>
    [CreateAssetMenu(fileName = "CriticalSettings", menuName = "Settings/Critical Settings")]
    public class CriticalSettings : ScriptableObject
    {
        [System.Serializable]
        public struct CriticalGradeSetting
        {
            public GrowthGrade Grade;
            [Tooltip("이 등급의 최소 치명타 확률/피해량 수치 (포함)")]
            public float MinValue;
            [Tooltip("이 등급의 최대 치명타 확률/피해량 수치 (포함)")]
            public float MaxValue;
        }
        
        [Header("치명타 확률 설정")]
        public CriticalGradeSetting[] CriticalRateSettings;

        [Header("치명타 피해량 설정")]
        public CriticalGradeSetting[] CriticalDamageSettings;
    }
}
