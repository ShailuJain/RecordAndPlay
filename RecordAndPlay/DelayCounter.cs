
using System.Threading;

namespace RecordAndPlay
{
    public static class DelayCounter
    {
        private static Timer timer;

        public static int Delay { get; private set; }
        static DelayCounter()
        {
            timer = new Timer(TimerCallbackFunc, null, 0, 10);
        }

        private static void TimerCallbackFunc(object state)
        {
            DelayCounter.Delay++;
        }
        public static void ResetDelay()
        {
            DelayCounter.Delay = 0;
        }
    }
}
