using message_bus.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Npgsql.Replication.PgOutput.Messages;
using scheduler.api.Data.DAL;
using scheduler.api.Data.DTO;
using scheduler.domain.CommandHandlers;
using scheduler.domain.Commands;

namespace scheduler.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchedulerController : Controller
{
    private readonly IMessageBus _messageBus;
    private readonly IJobRepository _jobRepository;
    private readonly ICommandHandler<JobCommand, bool> _jobCommand;

    public SchedulerController(IMessageBus messageBus, IJobRepository jobRepository, 
        ICommandHandler<JobCommand, bool> JobCommand)
    {
        _jobRepository = jobRepository;
        _messageBus = messageBus;
        _jobCommand = JobCommand;
    }

    [HttpGet]
    [Route("jobs")]
    public ActionResult<IEnumerable<JobDTO>> GetJobs()
    {
        return Ok(_jobRepository.GetJobs());
    }
    [HttpPost]
    public IActionResult Post([FromBody]  Job job)
    {

        JobCommands command = Enum.Parse<JobCommands>(job.Command, true);
        JobCommand cmd = new JobCommand(job.SchedName, job.JobName, job.JobGroupName, command);
        _jobCommand.Handle(cmd, CancellationToken.None);
        return Ok(job);
    }
}

public class Job
{
    public string SchedName { get; set; }
    public string JobName { get; set; }
    public string JobGroupName { get; set; }
    public string Command { get; set; }
}