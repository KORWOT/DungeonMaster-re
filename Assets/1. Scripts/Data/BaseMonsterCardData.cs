using System.Collections.Generic;
using DungeonMaster.Data.Enums;
using DungeonMaster.Data.Structs;

namespace DungeonMaster.Data
{
    /// <summary>
    /// 몬스터 카드의 모든 영구적인 데이터를 저장하는 클래스입니다.
    /// 이 데이터는 플레이어의 소유이며, 게임 세션 간에 유지됩니다.
    /// </summary>
    public class BaseMonsterCardData
    {
        // 기본 정보
        public string MonsterId;            // 몬스터의 종류를 식별하는 ID (예: "Goblin")
        public CardGrade BaseGrade;         // 카드의 기본 등급
        public ArmorType ArmorType;         // 방어구 타입
        public ElementType ElementType;     // 속성

        // 개별 성장률 (강화 가능)
        public GrowthRateData AttackGrowth;
        public GrowthRateData DefenseGrowth;
        public GrowthRateData HealthGrowth;

        // 개별 특성 (강화 가능)
        public CriticalData CriticalRate;
        public CriticalData CriticalDamage;

        // 고유 스킬들 (강화 가능)
        public List<UniqueSkillData> UniqueSkills;

        // 강화 관련
        public int EnhanceLevel;            // 전체 강화 레벨
        public long TotalExperience;        // 이 카드가 획득한 총 경험치
    }
}
