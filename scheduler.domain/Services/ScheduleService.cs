using scheduler.domain.CommandHandlers;
using scheduler.domain.Commands;
using scheduler.domain.Interfaces;

namespace scheduler.domain.Services;

public class ScheduleService : IScheduleService
{
    public ScheduleService()
    {
        
    }
    public void PauseJob(string schedName, string jobName, string jobGroupName, JobCommands command)
    {
        throw new NotImplementedException();
    }
}