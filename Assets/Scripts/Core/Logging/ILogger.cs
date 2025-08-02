using UnityEngine;

namespace Core.Logging
{
    /// <summary>
    /// 로그 출력을 위한 공통 인터페이스입니다.
    /// 이 인터페이스를 통해 로깅 시스템은 실제 출력 방식(콘솔, 파일, 서버 등)과 분리됩니다.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// 일반 정보 로그를 기록합니다.
        /// </summary>
        /// <param name="message">기록할 로그 메시지</param>
        /// <param name="context">로그 출처를 나타내는 Unity 오브젝트. 클릭 시 해당 오브젝트가 하이라이트됩니다.</param>
        void Log(string message, Object context = null);

        /// <summary>
        /// 경고 로그를 기록합니다.
        /// </summary>
        /// <param name="message">기록할 경고 메시지</param>
        /// <param name="context">로그 출처를 나타내는 Unity 오브젝트.</param>
        void LogWarning(string message, Object context = null);

        /// <summary>
        /// 오류 로그를 기록합니다.
        /// </summary>
        /// <param name="message">기록할 오류 메시지</param>
        /// <param name="context">로그 출처를 나타내는 Unity 오브젝트.</param>
        void LogError(string message, Object context = null);
    }
}
