using message_bus.Interfaces;
using scheduler.domain.Commands;
using scheduler.domain.Events;

namespace scheduler.domain.CommandHandlers;

public class JobCommandHandler : ICommandHandler<JobCommand, bool>
{
    private readonly IMessageBus _messageBus;
    public JobCommandHandler(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }
    public Task<bool> Handle(JobCommand command, CancellationToken cancellationToken)
    {
        var jobEvent = new JobEvent
        (
            command.SchedName,
            command.JobName,
            command.JobGroupName,
            command.Command
        );

        _messageBus.Publish(jobEvent);

        return Task.FromResult(true);
    }
}

