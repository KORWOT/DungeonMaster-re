using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly Dictionary<StatType, int> _finalStats = new Dictionary<StatType, int>();
        private readonly Dictionary<StatType, List<StatModifier>> _statModifiers = new Dictionary<StatType, List<StatModifier>>();
        
        // 스탯이 변경되었는지 여부를 추적하여 불필요한 계산을 방지합니다.
        private bool _isDirty = true;

        private void Awake()
        {
            InitializeStats();
        }

        private void InitializeStats()
        {
            foreach (StatType statType in Enum.GetValues(typeof(StatType)))
            {
                _finalStats[statType] = 0;
                _statModifiers[statType] = new List<StatModifier>();
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
            _statModifiers[modifier.StatType].Sort((a, b) => a.Type.CompareTo(b.Type));
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
            foreach (StatType statType in Enum.GetValues(typeof(StatType)))
            {
                _finalStats[statType] = CalculateFinalStat(statType);
            }
            GameLogger.Log("All final stats have been recalculated.");
        }

        private int CalculateFinalStat(StatType statType)
        {
            int finalValue = 0;
            // 1. 합연산(Flat) 적용
            foreach (var mod in _statModifiers[statType].Where(m => m.Type == StatModType.Flat))
            {
                finalValue += mod.Value;
            }
            
            // 2. 추가 백분율(PercentAdd) 적용
            int percentAddSum = 0;
            foreach (var mod in _statModifiers[statType].Where(m => m.Type == StatModType.PercentAdd))
            {
                percentAddSum += mod.Value;
            }
            finalValue += (int) (finalValue * (percentAddSum / 1000f));
            
            // 3. 최종 백분율(PercentMult) 적용
            foreach (var mod in _statModifiers[statType].Where(m => m.Type == StatModType.PercentMult))
            {
                finalValue = (int) (finalValue * (1 + mod.Value / 1000f));
            }

            return finalValue;
        }
    }
}
