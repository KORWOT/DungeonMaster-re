using UnityEngine;

namespace DungeonMaster.Core.Logging
{
    /// <summary>
    /// ILogger 인터페이스의 구현체로, Unity 에디터 콘솔에 로그를 출력합니다.
    /// </summary>
    public class UnityConsoleLogger : ILogger
    {
        /// <summary>
        /// 일반 로그를 Unity 콘솔에 출력합니다.
        /// </summary>
        /// <param name="message">출력할 메시지</param>
        /// <param name="context">로그와 연결할 Unity 오브젝트</param>
        public void Log(string message, Object context = null)
        {
            Debug.Log(message, context);
        }

        /// <summary>
        /// 경고 로그를 Unity 콘솔에 출력합니다.
        /// </summary>
        /// <param name="message">출력할 경고 메시지</param>
        /// <param name="context">로그와 연결할 Unity 오브젝트</param>
        public void LogWarning(string message, Object context = null)
        {
            Debug.LogWarning(message, context);
        }

        /// <summary>
        /// 오류 로그를 Unity 콘솔에 출력합니다.
        /// </summary>
        /// <param name="message">출력할 오류 메시지</param>
        /// <param name="context">로그와 연결할 Unity 오브젝트</param>
        public void LogError(string message, Object context = null)
        {
            Debug.LogError(message, context);
        }
    }
}
