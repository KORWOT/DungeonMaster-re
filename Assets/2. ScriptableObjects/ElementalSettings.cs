using DungeonMaster.Data.Enums;
using UnityEngine;

namespace Settings
{
    /// <summary>
    /// 속성 간의 상성 관계 및 효과를 정의하는 ScriptableObject입니다.
    /// </summary>
    [CreateAssetMenu(fileName = "ElementalSettings", menuName = "Settings/Elemental Settings")]
    public class ElementalSettings : ScriptableObject
    {
        // Unity는 2차원 배열을 직접 인스펙터에 노출하지 않으므로, 직렬화 가능한 클래스로 감싸서 표현합니다.
        [System.Serializable]
        public class DamageMultiplierRow
        {
            public float[] Defender = new float[8]; // 방어자 속성별 데미지 배율
        }
        
        [Header("속성 상성 매트릭스 (공격자 -> 방어자)")]
        [Tooltip("8x8 매트릭스. 각 행은 공격자 속성, 각 열은 방어자 속성에 해당합니다. 값은 데미지 배율입니다 (예: 1.5 = 150% 데미지).")]
        public DamageMultiplierRow[] Attacker = new DamageMultiplierRow[8];

        /// <summary>
        /// 두 속성 간의 데미지 배율을 가져옵니다.
        /// </summary>
        /// <param name="attacker">공격자 속성</param>
        /// <param name="defender">방어자 속성</param>
        /// <returns>데미지 배율</returns>
        public float GetDamageMultiplier(ElementType attacker, ElementType defender)
        {
            if (Attacker == null || Attacker.Length != 8 || Attacker[(int)attacker].Defender.Length != 8)
            {
                Debug.LogError("속성 상성 매트릭스가 올바르게 설정되지 않았습니다.");
                return 1f;
            }
            return Attacker[(int)attacker].Defender[(int)defender];
        }
    }
}
