using DungeonMaster.Data.Enums;

namespace DungeonMaster.Data.Structs
{
    /// <summary>
    /// 카드의 고유 스킬 데이터를 담는 구조체입니다.
    /// </summary>
    public struct UniqueSkillData
    {
        public string SkillId;      // 스킬 고유 ID
        public int SkillLevel;      // 스킬 레벨
    }
}
