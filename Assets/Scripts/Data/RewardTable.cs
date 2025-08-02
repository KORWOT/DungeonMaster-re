using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    /// <summary>
    /// 전투 보상에 대한 규칙과 데이터를 정의하는 ScriptableObject입니다.
    /// </summary>
    [CreateAssetMenu(fileName = "New RewardTable", menuName = "Data/Reward Table")]
    public class RewardTable : ScriptableObject
    {
        [Header("승리 보상")]
        [Tooltip("전투 승리 시 기본으로 지급되는 크리스탈")]
        public long BaseVictoryCrystal;
        [Tooltip("전투 승리 시 기본으로 지급되는 코인")]
        public long BaseVictoryCoin;

        [Header("패배 보상")]
        [Tooltip("도달한 웨이브 당 지급되는 크리스탈")]
        public long DefeatCrystalPerWave;
        [Tooltip("도달한 웨이브 당 지급되는 코인")]
        public long DefeatCoinPerWave;

        // TODO: 스테이지 난이도별 보상 배율, 카드/아이템 드랍 테이블 등 추가 가능
    }
}
