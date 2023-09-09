using message_bus.Events;
using scheduler.domain.Commands;

namespace scheduler.domain.Events;

public class JobEvent : Event
{
    public string SchedName { get; set; }
    public string JobName { get; set; }
    public string JobGroupName { get; set; }
    public JobCommands Command { get; set; }

    public JobEvent(string schedName, string jobName, string jobGroupName, JobCommands command)
    {
        SchedName = schedName;
        JobName = jobName;
        JobGroupName = jobGroupName;
        Command = command;
    }
}