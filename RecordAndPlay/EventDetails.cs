
namespace RecordAndPlay
{
    public class EventDetails
    {
        private EventType eventType;
        private long delay;
        public EventDetails(EventType type, long delay)
        {
            this.eventType = type;
            this.delay = delay;
        }
    }
    public enum EventType
    {
        MOUSEEVENT = 1,
        KEYBOARDEVENT = 2
    }
}
