namespace Core.Ticks
{
    /// <summary>
    /// TickManager가 발생시키는 틱 이벤트를 수신하는 모든 객체가 구현해야 하는 인터페이스입니다.
    /// </summary>
    public interface ITickListener
    {
        /// <summary>
        /// 틱이 발생할 때마다 호출됩니다.
        /// </summary>
        /// <param name="tick">현재 틱 수</param>
        void OnTick(long tick);
    }
}
