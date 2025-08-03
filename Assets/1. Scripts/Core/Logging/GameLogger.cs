using System;
using UnityEngine;

namespace Core.Logging
{
    /// <summary>
    /// 게임 전역에서 사용할 정적 로깅 클래스입니다.
    /// IsEnabled 플래그를 통해 조건부로 로그를 기록하여 성능 저하를 방지합니다.
    /// </summary>
    public static class GameLogger
    {
        private static ILogger _logger = new UnityConsoleLogger();

        /// <summary>
        /// 로그 기록을 활성화/비활성화하는 전역 스위치입니다.
        /// </summary>
        public static bool IsEnabled { get; set; }

        /// <summary>
        /// 정적 생성자. 에디터에서는 기본적으로 로그를 활성화하고, 빌드에서는 비활성화합니다.
        /// </summary>
        static GameLogger()
        {
#if UNITY_EDITOR
            IsEnabled = true;
#else
            IsEnabled = false;
#endif
        }

        /// <summary>
        /// 사용할 Logger 구현체를 설정합니다. null이 전달될 경우 기본 UnityConsoleLogger로 설정됩니다.
        /// </summary>
        public static void SetLogger(ILogger newLogger)
        {
            _logger = newLogger ?? new UnityConsoleLogger();
        }

        public static void Log(string message, UnityEngine.Object context = null)
        {
            if (!IsEnabled) return;
            _logger.Log($"[LOG] {DateTime.UtcNow:HH:mm:ss}: {message}", context);
        }

        public static void LogWarning(string message, UnityEngine.Object context = null)
        {
            if (!IsEnabled) return;
            _logger.LogWarning($"[WARNING] {DateTime.UtcNow:HH:mm:ss}: {message}", context);
        }

        public static void LogError(string message, Exception e = null, UnityEngine.Object context = null)
        {
            if (!IsEnabled) return;
            string errorMessage = $"[ERROR] {DateTime.UtcNow:HH:mm:ss}: {message}";
            if (e != null)
            {
                errorMessage += $"\nException: {e.Message}\n{e.StackTrace}";
            }
            _logger.LogError(errorMessage, context);
        }
    }
}
