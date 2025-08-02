using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// 게임의 핵심 서비스(매니저)들을 등록하고 제공하는 중앙 서비스 로케이터입니다.
    /// 싱글톤으로 구현되어 어디서든 접근 가능하며, 인터페이스 기반으로 서비스를 관리하여
    /// 코드 간의 결합도를 낮춥니다.
    /// </summary>
    public class Core : MonoBehaviour
    {
        public static Core Instance { get; private set; }

        // 등록된 서비스들을 타입별로 저장하는 딕셔너리
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        private void Awake()
        {
            if (Instance != null)
            {
                // 이미 인스턴스가 존재하면 이 오브젝트는 파괴
                Debug.LogWarning("Core 인스턴스가 이미 존재하여 새로 생성된 인스턴스를 파괴합니다.");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// 서비스를 Core에 등록합니다. 주로 각 서비스의 Awake()에서 호출됩니다.
        /// </summary>
        /// <typeparam name="T">등록할 서비스의 인터페이스 타입</typeparam>
        /// <param name="service">등록할 서비스 인스턴스</param>
        public void Register<T>(T service) where T : class
        {
            var type = typeof(T);
            if (_services.ContainsKey(type))
            {
                Debug.LogError($"서비스 등록 오류: 타입 '{type.Name}'의 서비스가 이미 등록되어 있습니다.");
                return;
            }
            _services[type] = service;
            Debug.Log($"서비스 등록됨: {type.Name}");
        }

        /// <summary>
        /// 등록된 서비스를 가져옵니다.
        /// </summary>
        /// <typeparam name="T">가져올 서비스의 인터페이스 타입</typeparam>
        /// <returns>등록된 서비스 인스턴스</returns>
        public T Get<T>() where T : class
        {
            var type = typeof(T);
            if (_services.TryGetValue(type, out var service))
            {
                return service as T;
            }
            
            Debug.LogError($"서비스 조회 오류: 타입 '{type.Name}'의 서비스를 찾을 수 없습니다. 등록되었는지 확인해주세요.");
            return null;
        }

        /// <summary>
        /// 등록된 서비스를 해제합니다. 주로 각 서비스의 OnDestroy()에서 호출됩니다.
        /// </summary>
        /// <typeparam name="T">등록 해제할 서비스의 인터페이스 타입</typeparam>
        public void Unregister<T>() where T : class
        {
            var type = typeof(T);
            if (_services.Remove(type))
            {
                Debug.Log($"서비스 등록 해제됨: {type.Name}");
            }
        }
    }
}
