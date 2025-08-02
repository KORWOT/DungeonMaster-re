using Character;
using Data;
using Settings;
using UnityEngine;

namespace Factories
{
    /// <summary>
    /// 영구 데이터(BaseMonsterCard 등)를 기반으로 실제 전투에 참여할 캐릭터 게임 오브젝트를 생성합니다.
    /// </summary>
    public class CharacterFactory
    {
        private readonly CardBalanceSettings _balanceSettings;

        public CharacterFactory(CardBalanceSettings balanceSettings)
        {
            _balanceSettings = balanceSettings;
        }

        /// <summary>
        /// 몬스터 베이스 카드 데이터로부터 몬스터 게임 오브젝트를 생성하고 초기화합니다.
        /// </summary>
        /// <param name="baseCard">생성할 몬스터의 베이스 카드 데이터</param>
        /// <returns>생성되고 초기화된 몬스터 게임 오브젝트</returns>
        public GameObject CreateCharacterFromBaseCard(BaseMonsterCard baseCard)
        {
            if (baseCard == null)
            {
                Debug.LogError("BaseMonsterCard 데이터가 null입니다.");
                return null;
            }

            // 1. 몬스터 ID에 해당하는 프리팹 로드
            // 실제 경로와 로드 방식은 프로젝트 구조에 맞게 변경 필요 (예: Addressables)
            var prefab = Resources.Load<GameObject>($"Prefabs/Monsters/{baseCard.MonsterId}");
            if (prefab == null)
            {
                Debug.LogError($"{baseCard.MonsterId}에 해당하는 몬스터 프리팹을 찾을 수 없습니다.");
                return null;
            }

            var monsterObject = Object.Instantiate(prefab);
            
            // 2. CharacterStats 컴포넌트 가져오기
            var statsComponent = monsterObject.GetComponent<CharacterStats>();
            if (statsComponent == null)
            {
                Debug.LogError($"{baseCard.MonsterId} 프리팹에 CharacterStats 컴포넌트가 없습니다.");
                Object.Destroy(monsterObject);
                return null;
            }
            
            // 3. 베이스 스탯 적용
            ApplyBaseStats(statsComponent, baseCard);

            // TODO: 속성/아머 효과 적용 로직 추가
            // ApplyElementalProperties(statsComponent, baseCard.ElementType);
            // ApplyArmorProperties(statsComponent, baseCard.ArmorType);
            
            Debug.Log($"{baseCard.MonsterId} 캐릭터가 생성되었습니다.");
            return monsterObject;
        }

        /// <summary>
        /// 카드의 데이터를 기반으로 캐릭터의 '기본' 스탯을 설정합니다.
        /// 이 값들은 스탯 계산의 기초가 되며, 모디파이어가 아닙니다.
        /// </summary>
        private void ApplyBaseStats(CharacterStats stats, BaseMonsterCard card)
        {
            // 카드 등급에 따른 기본 스탯 가져오기
            var gradeSetting = _balanceSettings.CardGradeSettings.GetSetting(card.BaseGrade);
            
            // TODO: LevelUpProcessor를 통해 계산된 스탯 증가량을 가져와야 함.
            // 현재는 등급에 따른 기본 스탯만 설정합니다.
            // int finalHealth = gradeSetting.BaseHealth + healthFromLevels;
            
            stats.SetBaseStat(StatType.MaxHP, gradeSetting.BaseHealth);
            stats.SetBaseStat(StatType.Attack, gradeSetting.BaseAttack);
            stats.SetBaseStat(StatType.Defense, gradeSetting.BaseDefense);
        }
    }
}
