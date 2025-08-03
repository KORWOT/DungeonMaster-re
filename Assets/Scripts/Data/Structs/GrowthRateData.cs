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
}
