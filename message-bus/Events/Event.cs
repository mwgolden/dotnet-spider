namespace message_bus.Events;

public abstract class Event
{
    public DateTime Timestamp { get; set; }

    protected Event()
    {
        Timestamp = DateTime.Now;
    }
}