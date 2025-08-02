using System.Collections.Generic;
using Core.Logging;

namespace Character
{
    /// <summary>
    /// 단일 스탯의 기본 값, 모디파이어 리스트를 관리하고 최종 값을 계산하는 클래스입니다.
    /// 기획서 9.1. '스탯 모디파이어' 패턴에 따라 각 스탯은 이 클래스의 인스턴스로 표현됩니다.
    /// </summary>
    [System.Serializable]
    public class CharacterStat
    {
        /// <summary>
        /// 스탯의 기본 값입니다. (예: 몬스터의 기본 공격력)
        /// </summary>
        public int BaseValue;

        /// <summary>
        /// 계산된 최종 스탯 값을 반환합니다.
        /// 변경이 있을 때만 값을 다시 계산하여 성능을 최적화합니다.
        /// </summary>
        public int Value
        {
            get
            {
                if (_isDirty)
                {
                    _lastValue = CalculateFinalValue();
                    _isDirty = false;
                }
                return _lastValue;
            }
        }

        private bool _isDirty = true;
        private int _lastValue;

        private readonly List<StatModifier> _statModifiers;

        public CharacterStat(int baseValue)
        {
            BaseValue = baseValue;
            _statModifiers = new List<StatModifier>();
        }
        
        /// <summary>
        /// 이 스탯에 새로운 모디파이어를 추가합니다. (예: 버프 적용)
        /// </summary>
        /// <param name="mod">추가할 스탯 모디파이어</param>
        public void AddModifier(StatModifier mod)
        {
            _isDirty = true;
            _statModifiers.Add(mod);
            // 계산 순서를 보장하기 위해 모디파이어 타입을 기준으로 정렬합니다.
            // Flat -> PercentAdd -> PercentMult 순서로 적용됩니다.
            _statModifiers.Sort((a, b) => a.Type.CompareTo(b.Type));
        }

        /// <summary>
        /// 특정 출처(source)에서 온 모든 모디파이어를 제거합니다. (예: 버프 해제)
        /// </summary>
        /// <param name="source">제거할 모디파이어의 출처</param>
        /// <returns>제거 여부</returns>
        public bool RemoveModifiersFromSource(object source)
        {
            int numRemoved = _statModifiers.RemoveAll(mod => mod.Source == source);
            
            if (numRemoved > 0)
            {
                _isDirty = true;
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// 최종 스탯 값을 계산합니다.
        /// 계산 순서: (기본 값 + 모든 Flat 합) * (1 + 모든 PercentAdd 합) * (1 + PercentMult 1) * (1 + PercentMult 2) ...
        /// 모든 연산은 결정론적 결과를 위해 정수 연산으로만 처리됩니다.
        /// </summary>
        /// <returns>계산된 최종 스탯 값</returns>
        private int CalculateFinalValue()
        {
            int finalValue = BaseValue;
            int percentAddSum = 0;

            // 리스트가 정렬되어 있으므로, 순서대로 순회하며 계산합니다.
            for (int i = 0; i < _statModifiers.Count; i++)
            {
                StatModifier mod = _statModifiers[i];

                if (mod.Type == StatModType.Flat)
                {
                    finalValue += mod.Value;
                }
                else if (mod.Type == StatModType.PercentAdd)
                {
                    percentAddSum += mod.Value;
                }
                else if (mod.Type == StatModType.PercentMult)
                {
                    // PercentMult를 적용하기 전에, 이전에 누적된 PercentAdd를 먼저 계산합니다.
                    if (percentAddSum != 0)
                    {
                        finalValue += (finalValue * percentAddSum) / 1000;
                        percentAddSum = 0; // 적용 후 초기화
                    }
                    finalValue = (finalValue * (1000 + mod.Value)) / 1000;
                }
            }
            
            // 모든 순회가 끝난 후, PercentMult 타입이 없어서 적용되지 않은 PercentAdd가 있다면 마저 계산합니다.
            if (percentAddSum != 0)
            {
                finalValue += (finalValue * percentAddSum) / 1000;
            }

            return finalValue;
        }
    }
}
