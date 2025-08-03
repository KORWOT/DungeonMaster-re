namespace DungeonMaster.Data.Enums
{
    /// <summary>
    /// 캐릭터가 가질 수 있는 모든 스탯의 종류를 정의합니다.
    /// </summary>
    public enum StatType
    {
        // --- 기본 정보 (0-99) ---
        Level = 0,
        Experience = 1,

        // --- 생존 스탯 (100-199) ---
        MaxHP = 101,              // 최대 생명력
        Defense = 102,            // 방어력
        ProtectionRate = 103,     // 보호율 (%)
        DamageReduction = 104,    // 피해 감소 (고정 수치)
        DamageReductionRate = 105,// 피해 감소율 (%)
        EvasionRate = 106,        // 회피율 (%)
        HealBonus = 107,          // 받는 회복량 증가 (%)
        LifeOnKill = 108,         // 처치 시 생명력 회복 (고정 수치)

        // --- 공격 스탯 (200-299) ---
        Attack = 200,             // 공격력
        CritRate = 201,           // 치명타율 (%)
        CritDamageBonus = 202,    // 치명타 피해 배율 (%)
        DamageBonus = 203,        // 최종 피해 증가 (%)
        Penetration = 204,        // 방어 관통력 (고정 수치)
        PenetrationRate = 205,    // 방어 관통률 (%)
        LifeSteal = 206,          // 흡혈률 (%)
        AttackSpeed = 207,        // 공격 속도 (틱 당 행동 빈도)
        CooldownReduction = 208,  // 재사용 대기시간 감소 (%)

        // --- 속성 관련 스탯 (300-399) ---
        AllElementalDamageBonus = 300, // 모든 속성 피해 증가 (%)
        FireDamageBonus = 301,         // 화속성 피해 증가 (%)
        WaterDamageBonus = 302,        // 수속성 피해 증가 (%)
        WindDamageBonus = 303,         // 풍속성 피해 증가 (%)
        EarthDamageBonus = 304,        // 지(地)속성 피해 증가 (%)
        LightDamageBonus = 305,        // 명속성 피해 증가 (%)
        DarkDamageBonus = 306,         // 암속성 피해 증가 (%)
    }
}
