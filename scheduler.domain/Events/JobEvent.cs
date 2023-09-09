using message_bus.Events;
using scheduler.domain.Commands;

namespace scheduler.domain.Events;

public class JobEvent : Event
{
    public string SchedName { get; set; }
    public string JobName { get; set; }
    public string JobGroupName { get; set; }
    public JobCommandEnums CommandEnum { get; set; }

    public JobEvent(string schedName, string jobName, string jobGroupName, JobCommandEnums commandEnum)
    {
        SchedName = schedName;
        JobName = jobName;
        JobGroupName = jobGroupName;
        CommandEnum = commandEnum;
    }
}