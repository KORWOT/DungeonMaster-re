namespace DungeonMaster.Data.Enums
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
}
