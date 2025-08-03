using UnityEngine;

namespace DungeonMaster.Settings
{
    /// <summary>
    /// 스킬의 레벨별 효과 및 데이터를 정의하는 ScriptableObject입니다.
    /// </summary>
    [CreateAssetMenu(fileName = "SkillSettings", menuName = "Settings/Skill Settings")]
    public class SkillSettings : ScriptableObject
    {
        [Header("스킬 기본 정보")]
        public string SkillId;
        public string SkillName;
        [TextArea] public string Description;

        [Header("레벨별 효과")]
        [Tooltip("스킬 레벨별 효과를 정의합니다. 배열의 인덱스가 (레벨 - 1)에 해당합니다.")]
        public SkillLevelData[] LevelEffects;

        [System.Serializable]
        public struct SkillLevelData
        {
            [Tooltip("스킬의 효과 값 (예: 데미지, 힐량, 지속시간 등)")]
            public float Value;
            [Tooltip("스킬의 쿨다운 시간")]
            public float Cooldown;
            // 필요에 따라 더 많은 필드 추가 가능 (예: 범위, 타겟 수 등)
        }
    }
}
