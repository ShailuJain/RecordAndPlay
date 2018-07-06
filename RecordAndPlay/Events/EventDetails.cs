
namespace Events
{
    public class EventDetails
    {
        public EventType eventType { get; }
        public int delay { get; }
        public EventDetails(EventType type, int delay)
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
