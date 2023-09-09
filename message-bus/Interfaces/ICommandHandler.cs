using message_bus.Commands;

namespace message_bus.Interfaces;

public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : Command<TResponse>
{
    Task<bool> Handle(TCommand command, CancellationToken cancellationToken);
}