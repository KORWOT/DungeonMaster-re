using System;
using System.Collections.Generic;
using Character;
using Core.Logging;
using UnityEngine;

namespace Character
{
    /// <summary>
    /// 스탯 변경 유형을 정의하는 열거형입니다.
    /// </summary>
    public enum StatModType
    {
        /// <summary>
        /// 고정 수치만큼 더합니다 (예: 힘 +10).
        /// </summary>
        Flat,
        /// <summary>
        /// 기본 스탯에 대한 백분율만큼 더합니다 (예: 기본 공격력의 20% 증가).
        /// </summary>
        PercentAdd,
        /// <summary>
        /// 모든 계산이 끝난 최종 값에 백분율로 곱합니다 (예: 최종 피해량 1.5배).
        /// </summary>
        PercentMult
    }

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
    
    /// <summary>
    /// 캐릭터의 모든 스탯을 관리하고 최종 스탯 값을 계산합니다.
    /// 모든 스탯 값은 결정론적 계산을 위해 정수(int)로 관리됩니다.
    /// 비율(%)이나 소수점이 필요한 스탯은 1000을 곱한 값으로 저장합니다. (예: 25.5% -> 255)
    /// </summary>
    public class CharacterStats : MonoBehaviour
    {
        [Tooltip("캐릭터의 기본 스탯 목록입니다. 'Flat' 타입으로 적용됩니다.")]
        [SerializeField] private List<StatModifier> baseStats = new List<StatModifier>();

        // 클래스 레벨에서 StatType 값들을 한 번만 캐싱하여 Enum.GetValues() 호출로 인한 GC 부담을 줄입니다.
        private static readonly StatType[] AllStatTypes = (StatType[])Enum.GetValues(typeof(StatType));

        private readonly Dictionary<StatType, int> _finalStats = new Dictionary<StatType, int>();
        private readonly Dictionary<StatType, List<StatModifier>> _statModifiers = new Dictionary<StatType, List<StatModifier>>();
        
        // 스탯이 변경되어 재계산이 필요한지를 나타내는 플래그입니다.
        private bool _isDirty = true;
        // 스탯별로 모디파이어 리스트의 정렬이 필요한지를 나타내는 플래그입니다.
        private readonly Dictionary<StatType, bool> _sortRequired = new Dictionary<StatType, bool>();

        private void Awake()
        {
            InitializeStats();
        }

        private void InitializeStats()
        {
            foreach (StatType statType in AllStatTypes)
            {
                _finalStats[statType] = 0;
                _statModifiers[statType] = new List<StatModifier>();
                _sortRequired[statType] = false;
            }

            foreach (var stat in baseStats)
            {
                // 기본 스탯은 이 컴포넌트 자신을 Source로 하여 Flat 타입으로 추가합니다.
                var modifier = new StatModifier(stat.StatType, stat.Value, StatModType.Flat, this);
                AddModifier(modifier);
            }
            
            _isDirty = true;
        }

        /// <summary>
        /// 지정된 스탯 타입의 최종 계산된 값을 반환합니다.
        /// </summary>
        /// <param name="statType">가져올 스탯의 타입</param>
        /// <returns>최종 스탯 값</returns>
        public int GetStat(StatType statType)
        {
            if (_isDirty)
            {
                CalculateAllFinalStats();
                _isDirty = false;
            }
            return _finalStats.TryGetValue(statType, out int value) ? value : 0;
        }

        /// <summary>
        /// 새로운 스탯 모디파이어를 추가합니다.
        /// </summary>
        /// <param name="modifier">추가할 스탯 모디파이어</param>
        public void AddModifier(StatModifier modifier)
        {
            _statModifiers[modifier.StatType].Add(modifier);
            _sortRequired[modifier.StatType] = true;
            _isDirty = true;
            GameLogger.Log($"Added StatModifier: {modifier.StatType} {modifier.Type} {modifier.Value} from {modifier.Source}");
        }

        /// <summary>
        /// 특정 출처(Source)로부터 비롯된 모든 스탯 모디파이어를 제거합니다.
        /// </summary>
        /// <param name="source">제거할 모디파이어의 출처 객체</param>
        public void RemoveModifiersFromSource(object source)
        {
            bool removedAny = false;
            foreach (var statModifiers in _statModifiers.Values)
            {
                // 리스트를 뒤에서부터 순회하여 GC 할당 없이 안전하게 제거
                for (int i = statModifiers.Count - 1; i >= 0; i--)
                {
                    if (statModifiers[i].Source == source)
                    {
                        statModifiers.RemoveAt(i);
                        removedAny = true;
                    }
                }
            }

            if (removedAny)
            {
                _isDirty = true;
                GameLogger.Log($"Removed all StatModifiers from source: {source}");
            }
        }
        
        private void CalculateAllFinalStats()
        {
            foreach (StatType statType in AllStatTypes)
            {
                _finalStats[statType] = CalculateFinalStat(statType);
            }
            GameLogger.Log("All final stats have been recalculated.");
        }

        private int CalculateFinalStat(StatType statType)
        {
            // 정렬이 필요한 경우에만 정렬을 수행하여 불필요한 연산을 줄입니다.
            if (_sortRequired.TryGetValue(statType, out bool needsSort) && needsSort)
            {
                _statModifiers[statType].Sort((a, b) => a.Type.CompareTo(b.Type));
                _sortRequired[statType] = false;
            }

            int finalValue = 0;
            int percentAddSum = 0;
            var modifiers = _statModifiers[statType];

            // 1. Flat 합산 및 PercentAdd 총합 계산
            for (int i = 0; i < modifiers.Count; i++)
            {
                var mod = modifiers[i];
                if (mod.Type == StatModType.Flat)
                {
                    finalValue += mod.Value;
                }
                else if (mod.Type == StatModType.PercentAdd)
                {
                    percentAddSum += mod.Value;
                }
            }

            // 2. PercentAdd 적용 (모든 Flat 합산 값에 대해)
            if (percentAddSum > 0)
            {
                // 1000을 곱한 정수이므로 1000f로 나누어 비율로 만듭니다.
                finalValue += Mathf.RoundToInt(finalValue * (percentAddSum / 1000f));
            }

            // 3. PercentMult 순차 적용
            for (int i = 0; i < modifiers.Count; i++)
            {
                var mod = modifiers[i];
                if (mod.Type == StatModType.PercentMult)
                {
                    finalValue = Mathf.RoundToInt(finalValue * (1 + mod.Value / 1000f));
                }
            }

            return finalValue;
        }
    }
}
