using System;
using System.Collections.Generic;
using DungeonMaster.Core.Logging;
using DungeonMaster.Data;
using DungeonMaster.Data.Enums;
using UnityEngine;

namespace DungeonMaster.Gameplay.Actors
{
    /// <summary>
    /// 캐릭터의 모든 스탯 컨테이너 역할을 하는 클래스입니다. 
    /// 각 스탯은 CharacterStat 클래스의 인스턴스로 관리됩니다.
    /// 기획서 9.1. '스탯 모디파이어' 패턴 구현의 핵심 클래스입니다.
    /// </summary>
    public class CharacterStats : MonoBehaviour
    {
        // 모든 스탯을 StatType을 키로 하여 관리하는 딕셔너리입니다.
        private readonly Dictionary<StatType, CharacterStat> _stats = new Dictionary<StatType, CharacterStat>();

        private void Awake()
        {
            // 모든 StatType에 대해 CharacterStat 인스턴스를 생성하여 초기화합니다.
            // 이렇게 하면 런타임에 새로운 스탯 타입이 추가되더라도 코드 수정 없이 처리할 수 있습니다.
            foreach (StatType statType in Enum.GetValues(typeof(StatType)))
            {
                _stats.Add(statType, new CharacterStat(0)); // 기본값은 0으로 시작합니다.
            }
        }

        /// <summary>
        /// 지정된 스탯의 최종 값을 가져옵니다.
        /// </summary>
        /// <param name="statType">가져올 스탯의 타입</param>
        /// <returns>최종 계산된 스탯 값</returns>
        public int GetStat(StatType statType)
        {
            if (_stats.TryGetValue(statType, out CharacterStat stat))
            {
                return stat.Value;
            }

            GameLogger.LogWarning($"[CharacterStats] GetStat: A stat of type {statType} was not found. Returning 0.");
            return 0;
        }

        /// <summary>
        /// 지정된 스탯의 기본 값을 설정합니다.
        /// 이 메서드는 주로 캐릭터가 처음 생성될 때 기본 스탯을 설정하는 데 사용됩니다.
        /// </summary>
        /// <param name="statType">설정할 스탯의 타입</param>
        /// <param name="value">설정할 기본 값</param>
        public void SetBaseStat(StatType statType, int value)
        {
            if (_stats.TryGetValue(statType, out CharacterStat stat))
            {
                stat.BaseValue = value;
            }
            else
            {
                GameLogger.LogWarning($"[CharacterStats] SetBaseStat: A stat of type {statType} was not found.");
            }
        }

        /// <summary>
        /// 새로운 스탯 모디파이어를 추가합니다.
        /// 작업은 해당 CharacterStat 인스턴스에 위임됩니다.
        /// </summary>
        /// <param name="modifier">추가할 스탯 모디파이어</param>
        public void AddModifier(StatModifier modifier)
        {
            if (_stats.TryGetValue(modifier.StatType, out CharacterStat stat))
            {
                stat.AddModifier(modifier);
                GameLogger.Log($"Added Modifier to {modifier.StatType}: {modifier.Value} ({modifier.Type}) from {modifier.Source}");
            }
            else
            {
                GameLogger.LogWarning($"[CharacterStats] AddModifier: A stat of type {modifier.StatType} was not found.");
            }
        }

        /// <summary>
        /// 특정 출처(Source)로부터 비롯된 모든 스탯 모디파이어를 제거합니다.
        /// 이 작업은 모든 스탯에 대해 이루어집니다. (예: 버프 아이템 효과 동시 제거)
        /// </summary>
        /// <param name="source">제거할 모디파이어의 출처 객체</param>
        public void RemoveModifiersFromSource(object source)
        {
            bool removedAny = false;
            foreach (CharacterStat stat in _stats.Values)
            {
                if (stat.RemoveModifiersFromSource(source))
                {
                    removedAny = true;
                }
            }

            if(removedAny)
            {
                GameLogger.Log($"Removed all modifiers from source: {source}");
            }
        }
    }
}
