using message_bus.Events;

namespace scheduler_api;

public class TestMessage : Event
{
    public string Message { get; set;  }
}