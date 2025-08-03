using System.Collections.Generic;
using DungeonMaster.Data;
using UnityEngine;

namespace DungeonMaster.Settings
{
    /// <summary>
    /// 게임의 핵심 설정을 관리하는 ScriptableObject입니다.
    /// 레벨업 보너스, 배속 설정 등 게임의 주요 변수를 이곳에서 관리합니다.
    /// </summary>
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        [Header("레벨업 보상 설정")]
        [Tooltip("레벨업 시 플레이어에게 주어지는 기본 스탯 보너스 목록입니다.")]
        public List<StatModifier> LevelUpStatBonuses;

        // 추후 다른 게임 설정들이 이곳에 추가될 수 있습니다.
        // 예: [Header("시간 배속 설정")] public float[] TimeScaleOptions = { 1f, 1.5f, 2f };
    }
}
