namespace message_bus.Commands;

public class Command<TResponse>
{
    public DateTime Timestamp { get; set; }

    protected Command()
    {
        Timestamp = DateTime.Now;
    }
}