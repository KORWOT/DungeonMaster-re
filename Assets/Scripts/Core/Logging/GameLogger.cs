namespace Core.Logging
{
    /// <summary>
    /// 게임 전역에서 사용할 정적 로깅 클래스입니다.
    /// 이 클래스를 통해 모든 로그를 기록하며, 실제 로그 출력은 ILogger 구현체에 위임합니다.
    /// </summary>
    public static class GameLogger
    {
        private static ILogger _logger = new UnityConsoleLogger();

        /// <summary>
        /// 사용할 Logger 구현체를 설정합니다. (예: UnityConsoleLogger, FileLogger)
        /// </summary>
        public static void SetLogger(ILogger newLogger)
        {
            _logger = newLogger;
        }

        public static void Log(string message)
        {
            _logger.Log($"[LOG] {System.DateTime.Now:HH:mm:ss}: {message}");
        }

        public static void LogWarning(string message)
        {
            _logger.LogWarning($"[WARNING] {System.DateTime.Now:HH:mm:ss}: {message}");
        }

        public static void LogError(string message, System.Exception e = null)
        {
            string errorMessage = $"[ERROR] {System.DateTime.Now:HH:mm:ss}: {message}";
            if (e != null)
            {
                errorMessage += $"\nException: {e.Message}\n{e.StackTrace}";
            }
            _logger.LogError(errorMessage);
        }
    }
}
