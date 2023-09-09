using Microsoft.AspNetCore.Mvc;
using Quartz;
using scheduler.api.Data.DAL;
using scheduler.api.Data.DTO;
using scheduler.domain.Commands;
using scheduler.domain.Events;
using scheduler.domain.Interfaces;

namespace scheduler.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchedulerController : Controller
{
    private readonly IJobRepository _jobRepository;
    private readonly IScheduleService _scheduleService;

    public SchedulerController(IJobRepository jobRepository, IScheduleService scheduleService)
    {
        _jobRepository = jobRepository;
        _scheduleService = scheduleService;
    }

    [HttpGet]
    [Route("jobs")]
    public ActionResult<IEnumerable<JobDTO>> GetJobs()
    {
        return Ok(_jobRepository.GetJobs());
    }
    [HttpPost("/{schedulerName}/pause")]
    public IActionResult PostPauseJob(string schedulerName, [FromBody]  Job job)
    {
        JobEvent j = new JobEvent(schedName: schedulerName, jobName: job.Name, jobGroupName: job.Group,
            commandEnum: JobCommandEnums.Pause);
        _scheduleService.HandleJobEvent(j);
        return Ok(job);
    }
    [HttpPost("/{schedulerName}/resume")]
    public IActionResult PostResumeJob(string schedulerName, [FromBody]  Job job)
    {
        JobEvent j = new JobEvent(schedName: schedulerName, jobName: job.Name, jobGroupName: job.Group,
            commandEnum: JobCommandEnums.Resume);
        _scheduleService.HandleJobEvent(j);
        return Ok(job);
    }
}

public class Job
{
    public string Name { get; set; }
    public string Group { get; set; }
}