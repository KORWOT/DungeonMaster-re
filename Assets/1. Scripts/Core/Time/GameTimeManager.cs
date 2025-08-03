using UnityEngine;

namespace Core.Time
{
    /// <summary>
    /// 게임의 전반적인 시간 흐름을 관리하는 싱글턴 클래스입니다.
    /// 배속, 일시정지 등 시간과 관련된 모든 제어는 이 클래스를 통해 이루어져야 합니다.
    /// </summary>
    public class GameTimeManager : MonoBehaviour
    {
        public static GameTimeManager Instance { get; private set; }

        [Header("Time Scale Settings")]
        [Tooltip("게임 시간의 배율입니다. 1.0이 기본 속도입니다.")]
        [SerializeField] private float _timeScale = 1.0f;

        /// <summary>
        /// 현재 게임 시간 배율입니다. 0 이상의 값으로 설정할 수 있습니다.
        /// </summary>
        public float TimeScale
        {
            get => IsPaused ? 0f : _timeScale;
            set => _timeScale = Mathf.Max(0f, value);
        }

        /// <summary>
        /// 배속과 일시정지가 적용된 게임의 델타 타임입니다.
        /// Update() 루프에서 시간 계산 시 Time.deltaTime 대신 이 값을 사용해야 합니다.
        /// </summary>
        public float GameDeltaTime => UnityEngine.Time.deltaTime * TimeScale;

        /// <summary>
        /// 게임이 일시정지 상태인지 여부를 나타냅니다.
        /// </summary>
        public bool IsPaused { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// 게임을 일시정지합니다.
        /// </summary>
        public void Pause()
        {
            if (IsPaused) return;
            IsPaused = true;
            Core.Logging.GameLogger.Log("Game Paused.");
        }

        /// <summary>
        /// 게임 일시정지를 해제합니다.
        /// </summary>
        public void Resume()
        {
            if (!IsPaused) return;
            IsPaused = false;
            Core.Logging.GameLogger.Log("Game Resumed.");
        }

        /// <summary>
        /// 게임의 일시정지 상태를 토글합니다.
        /// </summary>
        public void TogglePause()
        {
            if (IsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
}
