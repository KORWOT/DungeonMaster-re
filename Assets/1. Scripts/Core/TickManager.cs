using System.Collections.Generic;
using DungeonMaster.Core.Logging;
using UnityEngine;

namespace DungeonMaster.Core.Ticks
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

        // HashSet을 사용하여 리스너를 관리합니다. 중복을 허용하지 않으며, 추가/제거/검색 성능이 평균 O(1)입니다.
        private readonly HashSet<ITickListener> _listeners = new HashSet<ITickListener>();
        
        // Update 루프 중에 리스너가 제거되는 경우를 안전하게 처리하기 위한 임시 리스트
        private readonly List<ITickListener> _listenersToRemove = new List<ITickListener>();
        private bool _isIterating;

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
            _tickTimer += UnityEngine.Time.deltaTime;
            while (_tickTimer >= _tickInterval)
            {
                _tickTimer -= _tickInterval;
                _currentTick++;
                
                _isIterating = true;
                foreach (var listener in _listeners)
                {
                    listener.OnTick(_currentTick);
                }
                _isIterating = false;

                // 순회가 끝난 후, 제거 목록에 있던 리스너들을 일괄 제거합니다.
                if (_listenersToRemove.Count > 0)
                {
                    foreach (var listener in _listenersToRemove)
                    {
                        _listeners.Remove(listener);
                    }
                    _listenersToRemove.Clear();
                }
            }
        }

        /// <summary>
        /// 틱 이벤트를 수신할 리스너를 등록합니다.
        /// </summary>
        public void RegisterListener(ITickListener listener)
        {
            if (listener == null) return;
            // HashSet의 Add 메서드는 이미 요소가 존재하면 false를 반환하고 추가하지 않습니다.
            if (_listeners.Add(listener))
            {
                GameLogger.Log($"Registered tick listener: {listener.GetType().Name}", (listener is MonoBehaviour mb) ? mb : null);
            }
        }

        /// <summary>
        /// 틱 이벤트 수신을 중단할 리스너를 제거합니다.
        /// </summary>
        public void UnregisterListener(ITickListener listener)
        {
            if (listener == null) return;
            
            // 순회 중에 제거가 요청되면, 임시 목록에 추가해두고 나중에 처리합니다.
            if (_isIterating)
            {
                if (_listeners.Contains(listener) && !_listenersToRemove.Contains(listener))
                {
                    _listenersToRemove.Add(listener);
                }
                return;
            }

            if (_listeners.Remove(listener))
            {
                 GameLogger.Log($"Unregistered tick listener: {listener.GetType().Name}", (listener is MonoBehaviour mb) ? mb : null);
            }
        }

        /// <summary>
        /// 현재 틱 수를 반환합니다.
        /// </summary>
        public long GetCurrentTick() => _currentTick;
    }
}
