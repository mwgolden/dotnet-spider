using message_bus.Commands;
using message_bus.Events;

namespace message_bus.Interfaces;

public interface IMessageBus
{
    void Publish<T>(T @event) where T : Event;
    void Subscribe<T, TH>() where T : Event where TH : IEventHandler;
}