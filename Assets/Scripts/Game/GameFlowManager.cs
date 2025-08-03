using DungeonMaster.Core.Interfaces;
using DungeonMaster.Core.Logging;
using DungeonMaster.Data;
using DungeonMaster.Gameplay.Dungeon;
using DungeonMaster.Gameplay.Actors;
using DungeonMaster.Systems.Rewards;
using UnityEngine;

namespace DungeonMaster.Game
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
        //[SerializeField] private CharacterFactory characterFactory; // TODO: CharacterFactory 구현 후 주석 해제
        [SerializeField] private EnemySpawner enemySpawner;

        [Header("Data Assets (Resources 폴더)")]
        [Tooltip("사용할 보상 테이블 파일의 이름")]
        [SerializeField] private string rewardTableName = "DefaultRewardTable";
        
        // --- 서비스 로케이터를 통해 접근할 매니저들 ---
        private ISaveLoadManager _saveLoadManager;
        private IRewardManager _rewardManager;
        private IScoreManager _scoreManager;

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
            
            InitializeManagers();
            
            // 다른 컴포넌트 초기화
            // dungeonGrid와 placementManager는 MonoBehaviour가 아니므로 new로 생성해야 합니다.
            dungeonGrid = new DungeonGrid();
            placementManager = new MonsterPlacementManager(dungeonGrid);
            enemySpawner.Initialize(dungeonGrid);
        }

        private void InitializeManagers()
        {
            // 데이터 에셋 로드
            var rewardTable = Resources.Load<RewardTableData>(rewardTableName);
            if (rewardTable == null)
            {
                GameLogger.LogError($"RewardTableData '{rewardTableName}'을 Resources 폴더에서 찾을 수 없습니다!");
            }
            
            // 매니저 인스턴스를 생성하고 Core에 등록합니다.
            _saveLoadManager = new SaveLoadManager();
            Core.Instance.Register<ISaveLoadManager>(_saveLoadManager);

            _rewardManager = new RewardManager(rewardTable);
            Core.Instance.Register<IRewardManager>(_rewardManager);

            _scoreManager = new ScoreManager(rewardTable, 1); // 임시로 난이도 1
            Core.Instance.Register<IScoreManager>(_scoreManager);
        }

        private void Start()
        {
            // 초기 게임 상태를 로비로 설정
            _saveLoadManager.LoadData();
            _currentPlayerData = _saveLoadManager.LoadedData;

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
                    _scoreManager.ResetScore();
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
                    _rewardManager.DistributeRewards(_scoreManager.TotalScore, _currentPlayerData);
                    // 변경된 플레이어 데이터 저장
                    _saveLoadManager.SaveData(_currentPlayerData);
                    break;
                case GameState.Paused:
                    // 게임 일시정지 (GameTimeManager 사용)
                    break;
            }
        }
    }
}
