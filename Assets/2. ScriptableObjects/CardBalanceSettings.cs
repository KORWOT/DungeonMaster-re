using UnityEngine;

namespace Settings
{
    /// <summary>
    /// 게임의 모든 핵심 밸런스 설정 ScriptableObject들을 중앙에서 관리하고 참조하는 허브 역할을 합니다.
    /// </summary>
    [CreateAssetMenu(fileName = "CardBalanceSettings", menuName = "Settings/Card Balance Settings")]
    public class CardBalanceSettings : ScriptableObject
    {
        [Header("핵심 밸런스 데이터 참조")]
        [Tooltip("속성 상성 관계 설정")]
        public ElementalSettings ElementalSettings;

        [Tooltip("방어구 타입별 효과 설정")]
        public ArmorSettings ArmorSettings;
        
        [Tooltip("성장률 등급별 수치 범위 설정")]
        public GrowthRateSettings GrowthRateSettings;

        [Tooltip("치명타 등급별 수치 범위 설정")]
        public CriticalSettings CriticalSettings;

        [Tooltip("카드 등급별 기본 스탯 설정")]
        public CardGradeSettings CardGradeSettings;

        [Header("전체 밸런스 조절 변수")]
        [Tooltip("게임 전체의 공격력에 영향을 미치는 배율. 1.0이 기본값.")]
        [Range(0.1f, 5f)]
        public float GlobalAttackMultiplier = 1.0f;

        [Tooltip("게임 전체의 체력에 영향을 미치는 배율. 1.0이 기본값.")]
        [Range(0.1f, 5f)]
        public float GlobalHealthMultiplier = 1.0f;
    }
}
