using System.Collections.Generic;
using Character;
using Core.Time;
using UnityEngine;

namespace Combat.Buffs
{
    /// <summary>
    /// 캐릭터에 적용된 모든 버프/디버프를 관리하는 컴포넌트입니다.
    /// 버프의 추가, 제거, 시간 관리 등을 책임집니다.
    /// </summary>
    public class BuffController : MonoBehaviour
    {
        // 현재 활성화된 버프 인스턴스를 관리하는 리스트
        private readonly List<ActiveBuff> _activeBuffs = new List<ActiveBuff>();
        
        // 캐싱된 컴포넌트
        private CharacterStats _characterStats;

        private void Awake()
        {
            _characterStats = GetComponent<CharacterStats>();
            if (_characterStats == null)
            {
                Core.Logging.GameLogger.LogError($"CharacterStats 컴포넌트를 찾을 수 없습니다!", null, this);
                enabled = false;
            }
        }

        private void Update()
        {
            // 게임 시간을 기준으로 각 버프의 남은 시간을 갱신합니다.
            float deltaTime = GameTimeManager.Instance.GameDeltaTime;
            
            // 역순으로 순회하여 버프 제거 시에도 안전하도록 처리
            for (int i = _activeBuffs.Count - 1; i >= 0; i--)
            {
                _activeBuffs[i].Tick(deltaTime);
                if (_activeBuffs[i].IsFinished)
                {
                    RemoveBuff(i);
                }
            }
        }

        /// <summary>
        /// 캐릭터에게 새로운 버프를 적용합니다.
        /// </summary>
        /// <param name="buffData">적용할 버프의 ScriptableObject</param>
        public void AddBuff(Buff buffData)
        {
            var newBuff = new ActiveBuff(buffData, _characterStats);
            _activeBuffs.Add(newBuff);
            
            Core.Logging.GameLogger.Log($"{buffData.BuffName} 버프가 적용되었습니다.", this);
        }

        private void RemoveBuff(int index)
        {
            var buffToRemove = _activeBuffs[index];
            buffToRemove.End(); // 버프 종료 로직 호출 (스탯 모디파이어 제거 등)
            _activeBuffs.RemoveAt(index);
            
            Core.Logging.GameLogger.Log($"{buffToRemove.BuffData.BuffName} 버프가 만료되었습니다.", this);
        }
    }

    /// <summary>
    /// 실제 캐릭터에게 적용되어 활성화된 버프의 인스턴스를 나타내는 클래스입니다.
    /// </summary>
    public class ActiveBuff
    {
        public Buff BuffData { get; }
        public float RemainingTime { get; private set; }
        public bool IsFinished { get; private set; }

        private readonly CharacterStats _ownerStats;

        public ActiveBuff(Buff buffData, CharacterStats ownerStats)
        {
            BuffData = buffData;
            _ownerStats = ownerStats;
            
            RemainingTime = BuffData.Duration;
            // 지속시간이 0 이하이면 영구 버프로 간주
            if (RemainingTime <= 0)
            {
                RemainingTime = float.MaxValue;
            }
            
            ApplyEffects();
        }

        /// <summary>
        /// 매 프레임 호출되어 버프의 남은 시간을 갱신합니다.
        /// </summary>
        public void Tick(float deltaTime)
        {
            RemainingTime -= deltaTime;
            if (RemainingTime <= 0)
            {
                IsFinished = true;
            }
        }
        
        /// <summary>
        /// 버프 효과(스탯 모디파이어)를 캐릭터에게 적용합니다.
        /// </summary>
        private void ApplyEffects()
        {
            foreach (var modifierData in BuffData.StatModifiers)
            {
                // ActiveBuff 인스턴스 자신을 Source로 전달하여 나중에 쉽게 제거할 수 있도록 함
                var modifierInstance = new StatModifier(modifierData.StatType, modifierData.Value, modifierData.Type, this);
                _ownerStats.AddModifier(modifierInstance);
            }
        }

        /// <summary>
        /// 버프 효과가 종료될 때 호출되어 적용했던 효과를 제거합니다.
        /// </summary>
        public void End()
        {
            _ownerStats.RemoveModifiersFromSource(this);
        }
    }
}
