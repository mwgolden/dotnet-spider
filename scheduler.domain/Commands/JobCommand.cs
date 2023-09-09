using message_bus.Commands;

namespace scheduler.domain.Commands;

public class JobCommand : Command<bool>
{
    public string SchedName { get; set; }
    public string JobName { get; set; }
    public string JobGroupName { get; set; }
    public JobCommands Command { get; set; }

    public JobCommand(string schedName, string jobName, string jobGroupName, JobCommands command)
    {
        SchedName = schedName;
        JobName = jobName;
        JobGroupName = jobGroupName;
        Command = command;
    }
}