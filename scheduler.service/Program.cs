
using message_bus;
using message_bus.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using scheduler.service;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<QuartzOptions>(context.Configuration.GetSection("Quartz"));
        services.AddQuartz(q =>
        {
            q.ScheduleJob<ExampleJob>(trigger =>
                trigger
                    .WithIdentity("Combined trigger configuration")
                    .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(5)))
                    .WithDailyTimeIntervalSchedule(x => x.WithInterval(10, IntervalUnit.Second))
                    .WithDescription("trigger configured for a job with a single call"));
        });
        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });
        services.AddSingleton<IMessageBus, RabbitMQBus>();
    })
    .Build();
    
host.Run();