using scheduler.domain.Commands;
using scheduler.domain.Events;

namespace scheduler.domain.Interfaces;

public interface IScheduleService
{
    public void HandleJobEvent(JobEvent job);
}