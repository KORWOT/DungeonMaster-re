using DungeonMaster.Data.Enums;

namespace DungeonMaster.Data.Structs
{
    /// <summary>
    /// 치명타 관련 정보를 담는 구조체입니다.
    /// </summary>
    public struct CriticalData
    {
        public float Value;         // 실제 치명타 확률 또는 피해량 수치
        public GrowthGrade Grade;   // 치명타 등급 (F ~ S)
    }
}
