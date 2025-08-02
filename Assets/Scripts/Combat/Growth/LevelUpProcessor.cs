using Data;
using Data.Structs;
using UnityEngine;

namespace Combat.Growth
{
    /// <summary>
    /// 레벨업에 따른 스탯 변화를 계산하고 처리하는 클래스입니다.
    /// </summary>
    public class LevelUpProcessor
    {
        /// <summary>
        /// 주어진 레벨만큼 카드의 스탯을 성장시킵니다.
        /// 현재는 단순 for문으로 구현하며, 추후 필요 시 성능 최적화를 진행합니다.
        /// </summary>
        /// <param name="card">성장시킬 카드</param>
        /// <param name="levelsToProcess">올릴 레벨의 수</param>
        public void ProcessLevelUp(BaseMonsterCard card, int levelsToProcess)
        {
            if (card == null || levelsToProcess <= 0) return;
            
            // TODO: 레벨업으로 인한 실제 스탯 변화를 어떻게 적용할지 정의 필요.
            // 예시: BaseMonsterCard에 BaseAttack, BaseHealth 등의 필드를 추가하고,
            // 이 메서드에서 해당 필드 값을 직접 증가시키는 방식.
            // 또는, 레벨업으로 인한 스탯 총 증가량을 반환하여 다른 곳에서 처리하게 할 수도 있음.
            
            long healthIncrease = 0;
            long attackIncrease = 0;
            long defenseIncrease = 0;

            for (int i = 0; i < levelsToProcess; i++)
            {
                // 각 성장률에 따라 스탯 증가량 누적
                // 실제 스탯이 float이라면 소수점 계산이 필요하지만,
                // 기획상 스탯이 정수(int, long)라면, 성장률 적용 방식을 명확히 해야 함.
                // 예: 성장률 0.5f => 2번 레벨업 시 스탯 1 증가
                healthIncrease += (long)card.HealthGrowth.Rate;
                attackIncrease += (long)card.AttackGrowth.Rate;
                defenseIncrease += (long)card.DefenseGrowth.Rate;
            }

            Debug.Log($"{card.MonsterId}가 {levelsToProcess}만큼 레벨업! " +
                      $"체력 +{healthIncrease}, 공격력 +{attackIncrease}, 방어력 +{defenseIncrease} (적용 로직 미구현)");
        }
    }
}
