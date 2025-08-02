using System.Collections.Generic;
using Core.Logging;
using Data;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// 전투 종료 후 보상 지급을 처리하는 클래스입니다.
    /// </summary>
    public class RewardManager : MonoBehaviour
    {
        // TODO: 보상 테이블(획득 가능 아이템, 재화 양 등)을 ScriptableObject로 관리
        // [SerializeField] private RewardTable rewardTable;

        /// <summary>
        /// 전투 승리 시 보상을 계산하고 지급합니다.
        /// </summary>
        /// <param name="playerData">플레이어의 영구 데이터</param>
        public void GrantVictoryRewards(PlayerData playerData)
        {
            GameLogger.Log("전투 승리! 보상을 지급합니다.");

            // TODO: 스테이지, 웨이브 등급 등에 따라 보상 차등 지급 로직 구현
            long crystalReward = 100; // 예시: 기본 크리스탈 보상
            long coinReward = 500;    // 예시: 기본 코인 보상

            playerData.Crystal += crystalReward;
            playerData.Coin += coinReward;
            
            GameLogger.Log($"보상: 크리스탈 +{crystalReward}, 코인 +{coinReward}");
            
            // TODO: 카드, 아이템 등 추가 보상 지급 로직
        }

        /// <summary>
        /// 전투 패배 시 보상을 계산하고 지급합니다.
        /// </summary>
        /// <param name="playerData">플레이어의 영구 데이터</param>
        /// <param name="reachedWave">도달한 웨이브 번호</param>
        public void GrantDefeatRewards(PlayerData playerData, int reachedWave)
        {
            GameLogger.Log($"전투 패배... (도달 웨이브: {reachedWave})");

            // TODO: 도달한 웨이브에 비례하여 보상 지급
            long crystalReward = reachedWave * 5;
            long coinReward = reachedWave * 20;
            
            playerData.Crystal += crystalReward;
            playerData.Coin += coinReward;

            GameLogger.Log($"보상: 크리스탈 +{crystalReward}, 코인 +{coinReward}");
        }
    }
}
