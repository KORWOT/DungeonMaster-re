using System.Collections.Generic;
using System.Linq;
using DungeonMaster.Gameplay.Actors;
using DungeonMaster.Core.Logging;
using DungeonMaster.Core.Time;
using UnityEngine;

namespace DungeonMaster.Gameplay.Combat
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
                GameLogger.LogError($"CharacterStats 컴포넌트를 찾을 수 없습니다!", null, this);
                enabled = false;
            }
        }

        private void Update()
        {
            // GameTimeManager 인스턴스가 없는 경우(예: 테스트 씬) 오류를 방지하기 위해 null 체크
            if (GameTimeManager.Instance == null)
            {
                return;
            }
            
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
        /// 중첩 규칙(StackingType)에 따라 다르게 동작합니다.
        /// </summary>
        /// <param name="buffData">적용할 버프의 ScriptableObject</param>
        public void AddBuff(BuffData buffData)
        {
            // 중복 허용이 아닌 경우, 먼저 동일한 버프가 있는지 확인합니다.
            if (buffData.StackingType != BuffStackingType.AllowDuplicate)
            {
                var existingBuff = _activeBuffs.FirstOrDefault(b => b.BuffData.BuffId == buffData.BuffId);
                if (existingBuff != null)
                {
                    // 기존 버프가 있다면 규칙에 따라 처리
                    switch (buffData.StackingType)
                    {
                        case BuffStackingType.RefreshDuration:
                            existingBuff.RefreshDuration();
                            GameLogger.Log($"{buffData.BuffName} 버프의 지속시간을 갱신합니다.", this);
                            return; // 새 버프를 추가하지 않고 종료
                        
                        case BuffStackingType.Ignore:
                            GameLogger.Log($"{buffData.BuffName} 버프는 이미 적용되어 있어 무시합니다.", this);
                            return; // 새 버프를 추가하지 않고 종료
                    }
                }
            }
            
            // 새 버프를 추가합니다.
            var newBuff = new ActiveBuff(buffData, _characterStats);
            _activeBuffs.Add(newBuff);
            
            GameLogger.Log($"{buffData.BuffName} 버프가 적용되었습니다.", this);
        }

        private void RemoveBuff(int index)
        {
            var buffToRemove = _activeBuffs[index];
            buffToRemove.End(); // 버프 종료 로직 호출 (스탯 모디파이어 제거 등)
            _activeBuffs.RemoveAt(index);
            
            GameLogger.Log($"{buffToRemove.BuffData.BuffName} 버프가 만료되었습니다.", this);
        }
    }

    /// <summary>
    /// 실제 캐릭터에게 적용되어 활성화된 버프의 인스턴스를 나타내는 클래스입니다.
    /// </summary>
    public class ActiveBuff
    {
        public BuffData BuffData { get; }
        public float RemainingTime { get; private set; }
        public bool IsFinished { get; private set; }

        private readonly CharacterStats _ownerStats;

        public ActiveBuff(BuffData buffData, CharacterStats ownerStats)
        {
            BuffData = buffData;
            _ownerStats = ownerStats;
            
            RefreshDuration();
            ApplyEffects();
        }

        /// <summary>
        /// 버프의 지속 시간을 초기값으로 재설정합니다.
        /// </summary>
        public void RefreshDuration()
        {
            RemainingTime = BuffData.Duration;
            // 지속시간이 0 이하이면 영구 버프로 간주
            if (RemainingTime <= 0)
            {
                RemainingTime = float.MaxValue;
            }
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
            // 이전에 적용된 같은 종류의 버프 효과가 있다면 제거 후 적용해야 하지만,
            // 현재 구조에서는 Source(ActiveBuff 인스턴스)가 다르므로 괜찮습니다.
            // 만약 source를 공유해야 한다면 다른 방식이 필요합니다.
            foreach (var modifierData in BuffData.StatModifiers)
            {
                var modifierInstance = new DungeonMaster.Data.StatModifier(modifierData.StatType, modifierData.Value, modifierData.Type, this);
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
