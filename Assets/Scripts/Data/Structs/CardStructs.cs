using DungeonMaster.Data.Enums;

namespace DungeonMaster.Data.Structs
{
    /// <summary>
    /// 성장률 정보를 담는 구조체입니다.
    /// </summary>
    public struct GrowthRateData
    {
        public float Rate;          // 실제 성장률 수치
        public GrowthGrade Grade;   // 성장률 등급 (F ~ S)
    }

    /// <summary>
    /// 치명타 관련 정보를 담는 구조체입니다.
    /// </summary>
    public struct CriticalData
    {
        public float Value;         // 실제 치명타 확률 또는 피해량 수치
        public GrowthGrade Grade;   // 치명타 등급 (F ~ S)
    }

    /// <summary>
    /// 카드의 고유 스킬 데이터를 담는 구조체입니다.
    /// </summary>
    public struct UniqueSkillData
    {
        public string SkillId;      // 스킬 고유 ID
        public int SkillLevel;      // 스킬 레벨
    }
}
