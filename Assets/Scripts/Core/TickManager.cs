using System.Collections.Generic;
using UnityEngine;

namespace Core.Ticks
{
    /// <summary>
    /// 게임의 전반적인 틱(시간 단위)을 관리하는 싱글턴 클래스입니다.
    /// 전투, 버프/디버프 지속 시간 등 모든 시간 기반 로직의 기준이 됩니다.
    /// </summary>
    public class TickManager : MonoBehaviour
    {
        public static TickManager Instance { get; private set; }

        [Tooltip("1초당 발생하는 틱의 수입니다. 이 값이 높을수록 게임의 시간 해상도가 높아집니다.")]
        [SerializeField] private float ticksPerSecond = 10f;

        private long _currentTick;
        private float _tickTimer;
        private float _tickInterval;

        private readonly List<ITickListener> _listeners = new List<ITickListener>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 전환되어도 파괴되지 않도록 설정
            
            _tickInterval = 1f / ticksPerSecond;
        }

        private void Update()
        {
            _tickTimer += Time.deltaTime;
            while (_tickTimer >= _tickInterval)
            {
                _tickTimer -= _tickInterval;
                _currentTick++;
                
                // 역순으로 순회하여 리스너가 OnTick 내부에서 스스로를 제거하더라도 안전합니다.
                for (int i = _listeners.Count - 1; i >= 0; i--)
                {
                    _listeners[i].OnTick(_currentTick);
                }
            }
        }

        /// <summary>
        /// 틱 이벤트를 수신할 리스너를 등록합니다.
        /// </summary>
        public void RegisterListener(ITickListener listener)
        {
            if (!_listeners.Contains(listener))
            {
                _listeners.Add(listener);
            }
        }

        /// <summary>
        /// 틱 이벤트 수신을 중단할 리스너를 제거합니다. Remove는 내부적으로 탐색 후 제거하므로 효율적입니다.
        /// </summary>
        public void UnregisterListener(ITickListener listener)
        {
            _listeners.Remove(listener);
        }

        /// <summary>
        /// 현재 틱 수를 반환합니다.
        /// </summary>
        public long GetCurrentTick() => _currentTick;
    }
}
