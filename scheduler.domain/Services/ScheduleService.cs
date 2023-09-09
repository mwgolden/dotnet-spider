using message_bus.Interfaces;
using scheduler.domain.Commands;
using scheduler.domain.Events;
using scheduler.domain.Interfaces;
namespace scheduler.domain.Services;

public class ScheduleService : IScheduleService
{
    private readonly IMessageBus _messageBus;
    private readonly ICommandHandler<JobCommand, bool> _jobCommand;
    public ScheduleService(IMessageBus messageBus,
            ICommandHandler<JobCommand, bool> jobCommand)
    {
        _messageBus = messageBus;
        _jobCommand = jobCommand;
    }
    public void HandleJobEvent(JobEvent job)
    {
        JobCommand cmd = new JobCommand(job.SchedName, job.JobName, job.JobGroupName, job.CommandEnum);
        _jobCommand.Handle(cmd, CancellationToken.None);
    }
}