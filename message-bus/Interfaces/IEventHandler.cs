using message_bus.Events;

namespace message_bus.Interfaces;

public interface IEventHandler<in TEvent> : IEventHandler
    where TEvent : Event
{
    Task Hanlde(TEvent @event);
}

public interface IEventHandler { }