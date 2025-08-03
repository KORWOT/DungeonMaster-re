using System;
using DungeonMaster.Data.Enums;

namespace DungeonMaster.Data
{
    /// <summary>
    /// 단일 스탯 수치 변경을 나타내는 클래스입니다.
    /// </summary>
    [Serializable]
    public class StatModifier
    {
        public StatType StatType;
        public int Value;
        public StatModType Type;
        /// <summary>
        /// 이 스탯 모디파이어를 제공한 출처입니다 (예: 버프, 장비 아이템).
        /// </summary>
        public readonly object Source;

        public StatModifier(StatType statType, int value, StatModType type, object source)
        {
            StatType = statType;
            Value = value;
            Type = type;
            Source = source;
        }
    }
}
