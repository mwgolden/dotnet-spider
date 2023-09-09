using scheduler.domain.Commands;

namespace scheduler.domain.Interfaces;

public interface IScheduleService
{
    public void PauseJob(string schedName, string jobName, string jobGroupName, JobCommands command);
}