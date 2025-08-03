using DungeonMaster.Data.Enums;
using UnityEngine;

namespace Settings
{
    /// <summary>
    /// 방어구 타입별 효과(보호율, 공격속도 변화 등)를 정의하는 ScriptableObject입니다.
    /// </summary>
    [CreateAssetMenu(fileName = "ArmorSettings", menuName = "Settings/Armor Settings")]
    public class ArmorSettings : ScriptableObject
    {
        [System.Serializable]
        public struct ArmorTypeSetting
        {
            public ArmorType Type;
            [Tooltip("보호율 (%): 이 비율만큼 데미지가 감소합니다.")]
            [Range(0f, 100f)]
            public float ProtectionRate;

            [Tooltip("공격 속도 변화율 (%): 양수는 증가, 음수는 감소를 의미합니다.")]
            public float AttackSpeedModifier;
        }

        public ArmorTypeSetting[] ArmorSettingsList;

        /// <summary>
        /// 특정 방어구 타입에 해당하는 설정을 찾습니다.
        /// </summary>
        public ArmorTypeSetting GetSetting(ArmorType armorType)
        {
            foreach (var setting in ArmorSettingsList)
            {
                if (setting.Type == armorType)
                {
                    return setting;
                }
            }
            // 설정이 없는 경우 기본값 반환
            Debug.LogWarning($"{armorType}에 대한 설정이 ArmorSettings에 없습니다.");
            return new ArmorTypeSetting { Type = armorType, ProtectionRate = 0, AttackSpeedModifier = 0 };
        }
    }
}
