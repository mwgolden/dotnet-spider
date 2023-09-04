using Quartz;

namespace scheduler_service;

public class ExampleJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await Console.Out.WriteLineAsync("Hello from the job");
    }
}