using System.ComponentModel;
using message_bus.Events;
using message_bus.Interfaces;
using Quartz;
using scheduler.domain.Commands;
using scheduler.domain.Events;

namespace scheduler.domain.EventHandlers;

public class JobEventHandler : IEventHandler<JobEvent>
{
    private readonly ISchedulerFactory _schedulerFactory;

    public JobEventHandler(ISchedulerFactory schedulerFactory)
    {
        _schedulerFactory = schedulerFactory;
    }
    public async Task Handle(JobEvent @event)
    {
        var scheduler = await _schedulerFactory.GetScheduler(@event.SchedName);
        JobKey jKey = new JobKey(@event.JobName, @event.JobGroupName);
        switch (@event.Command)
        {
            case JobCommands.Pause: PauseJob(scheduler, jKey);
                break;
            default:
                throw new InvalidEnumArgumentException();
        }
        
    }

    private void PauseJob(IScheduler scheduler, JobKey jKey)
    {
        scheduler?.PauseJob(jKey);
    }
}