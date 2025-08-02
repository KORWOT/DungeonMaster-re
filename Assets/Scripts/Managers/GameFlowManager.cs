using Combat;
using Core.Logging;
using Data;
using Dungeon;
using Factories;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// 게임의 전체적인 흐름과 상태를 관리하는 싱글톤 클래스입니다.
    /// </summary>
    public class GameFlowManager : MonoBehaviour
    {
        public static GameFlowManager Instance { get; private set; }

        public enum GameState
        {
            Lobby,          // 로비/준비 화면
            Placement,      // 던전 및 몬스터 배치 단계
            Combat,         // 전투 진행 단계
            Result,         // 전투 결과 (승리/패배) 표시 단계
            Paused          // 일시정지
        }

        public GameState CurrentState { get; private set; }

        // --- 참조 컴포넌트 (인스펙터에서 할당) ---
        [Header("Component References")]
        [SerializeField] private DungeonGrid dungeonGrid;
        [SerializeField] private MonsterPlacementManager placementManager;
        [SerializeField] private CharacterFactory characterFactory;
        [SerializeField] private EnemySpawner enemySpawner;

        [Header("Data Assets (Resources 폴더)")]
        [Tooltip("사용할 보상 테이블 파일의 이름")]
        [SerializeField] private string rewardTableName = "DefaultRewardTable";
        
        // --- 내부 관리 클래스 (직접 생성) ---
        public ScoreManager ScoreManager { get; private set; }
        public RewardManager RewardManager { get; private set; }
        public SaveLoadManager SaveLoadManager { get; private set; }

        // --- 임시 데이터 ---
        private PlayerData _currentPlayerData;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // 데이터 에셋 로드
            var rewardTable = Resources.Load<RewardTable>(rewardTableName);
            if (rewardTable == null)
            {
                GameLogger.LogError($"RewardTable '{rewardTableName}'을 Resources 폴더에서 찾을 수 없습니다!");
            }

            // MonoBehaviour가 아닌 매니저들은 직접 생성합니다.
            ScoreManager = new ScoreManager(rewardTable, 1); // 임시로 난이도 1
            RewardManager = new RewardManager(rewardTable);
            SaveLoadManager = new SaveLoadManager();
            
            // 다른 컴포넌트 초기화
            enemySpawner.Initialize(dungeonGrid);
        }

        private void Start()
        {
            // 초기 게임 상태를 로비로 설정
            _currentPlayerData = SaveLoadManager.LoadData() ?? new PlayerData();
            ChangeState(GameState.Lobby);
        }

        /// <summary>
        /// 게임 상태를 변경하고, 각 상태에 맞는 초기화 로직을 호출합니다.
        /// </summary>
        /// <param name="newState">변경할 새로운 게임 상태</param>
        public void ChangeState(GameState newState)
        {
            if (CurrentState == newState) return;

            CurrentState = newState;
            GameLogger.Log($"게임 상태 변경: {newState}");

            switch (CurrentState)
            {
                case GameState.Lobby:
                    // 로비 UI 표시, 모든 게임 데이터 초기화
                    ScoreManager.ResetScore();
                    break;
                case GameState.Placement:
                    // 배치 UI 활성화, 던전 그리드 초기화
                    // placementManager.ShowPlacementUI(...);
                    break;
                case GameState.Combat:
                    // 배치된 몬스터로 캐릭터 생성 (CharacterFactory 사용)
                    // 적 스폰 시작 (EnemySpawner 사용)
                    // enemySpawner.StartSpawning(...);
                    break;
                case GameState.Result:
                    // 전투 중단, 결과 UI 표시, 보상 처리
                    RewardManager.GrantRewards(_currentPlayerData, ScoreManager.TotalScore);
                    // 변경된 플레이어 데이터 저장
                    SaveLoadManager.SaveData(_currentPlayerData);
                    break;
                case GameState.Paused:
                    // 게임 일시정지 (GameTimeManager 사용)
                    break;
            }
        }
    }
}
