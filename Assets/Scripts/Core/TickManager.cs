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
            
            _tickInterval = 1f / ticksPerSecond;
        }

        private void Update()
        {
            _tickTimer += Time.deltaTime;
            while (_tickTimer >= _tickInterval)
            {
                _tickTimer -= _tickInterval;
                _currentTick++;
                
                for (int i = 0; i < _listeners.Count; i++)
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
        /// 틱 이벤트 수신을 중단할 리스너를 제거합니다.
        /// </summary>
        public void UnregisterListener(ITickListener listener)
        {
            if (_listeners.Contains(listener))
            {
                _listeners.Remove(listener);
            }
        }

        /// <summary>
        /// 현재 틱 수를 반환합니다.
        /// </summary>
        public long GetCurrentTick() => _currentTick;
    }
}
