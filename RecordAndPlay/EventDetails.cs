
namespace RecordAndPlay
{
    public class EventDetails
    {
        public EventType eventType { get; }
        public long delay { get; }
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
