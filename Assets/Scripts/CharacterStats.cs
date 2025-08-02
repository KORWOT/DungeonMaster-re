using System;
using System.Collections.Generic;
using Character;
using Core.Logging;
using UnityEngine;

namespace Character
{
    public enum StatModType
    {
        Flat,       // 합산 (e.g. 힘 +10)
        PercentAdd, // 추가 백분율 (e.g. 기본 공격력의 20% 증가)
        PercentMult // 최종 백분율 (e.g. 모든 계산 후 최종 피해량 1.5배)
    }

    [Serializable]
    public class StatModifier
    {
        public StatType StatType;
        public int Value;
        public StatModType Type;
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
        [SerializeField] private List<StatModifier> baseStats = new List<StatModifier>();

        // 클래스 레벨에서 StatType 값들을 한 번만 캐싱하여 Enum.GetValues() 호출로 인한 GC 부담을 줄입니다.
        private static readonly StatType[] AllStatTypes = (StatType[])Enum.GetValues(typeof(StatType));

        private readonly Dictionary<StatType, int> _finalStats = new Dictionary<StatType, int>();
        private readonly Dictionary<StatType, List<StatModifier>> _statModifiers = new Dictionary<StatType, List<StatModifier>>();
        
        private bool _isDirty = true;
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
                var modifier = new StatModifier(stat.StatType, stat.Value, StatModType.Flat, this);
                AddModifier(modifier);
            }
            
            _isDirty = true;
        }

        public int GetStat(StatType statType)
        {
            if (_isDirty)
            {
                CalculateAllFinalStats();
                _isDirty = false;
            }
            return _finalStats.TryGetValue(statType, out int value) ? value : 0;
        }

        public void AddModifier(StatModifier modifier)
        {
            _statModifiers[modifier.StatType].Add(modifier);
            _sortRequired[modifier.StatType] = true;
            _isDirty = true;
            GameLogger.Log($"Added StatModifier: {modifier.StatType} {modifier.Type} {modifier.Value} from {modifier.Source}");
        }

        public void RemoveModifiersFromSource(object source)
        {
            bool removed = false;
            foreach (var statModifiers in _statModifiers.Values)
            {
                int count = statModifiers.RemoveAll(mod => mod.Source == source);
                if (count > 0)
                {
                    removed = true;
                }
            }
            
            if (removed)
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
            if (_sortRequired.TryGetValue(statType, out bool needsSort) && needsSort)
            {
                _statModifiers[statType].Sort((a, b) => a.Type.CompareTo(b.Type));
                _sortRequired[statType] = false;
            }

            int finalValue = 0;
            int percentAddSum = 0;
            var modifiers = _statModifiers[statType];

            // 1. Flat 합산
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

            // 2. PercentAdd 적용
            if (percentAddSum > 0)
            {
                finalValue += Mathf.RoundToInt(finalValue * (percentAddSum / 1000f));
            }

            // 3. PercentMult 적용
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
