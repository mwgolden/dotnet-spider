using System.ComponentModel;
using message_bus.Events;
using message_bus.Interfaces;
using Quartz;
using scheduler.domain.Commands;
using scheduler.domain.Events;
using scheduler.domain.Interfaces;

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
        switch (@event.CommandEnum)
        {
            case JobCommandEnums.Pause: PauseJob(scheduler, jKey);
                break;
            case JobCommandEnums.Resume: ResumeJob(scheduler, jKey);
                break;
            default:
                throw new InvalidEnumArgumentException();
        }
        
    }

    private void PauseJob(IScheduler scheduler, JobKey jKey)
    {
        Console.WriteLine("Job will be paused");
        scheduler?.PauseJob(jKey);
    }

    private void ResumeJob(IScheduler scheduler, JobKey jKey)
    {
        Console.WriteLine("Job will be resumed");
        scheduler?.ResumeJob(jKey);
    }
}