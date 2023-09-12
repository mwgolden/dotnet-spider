using Microsoft.AspNetCore.Mvc;
using scheduler.api.Data.DAL;
using scheduler.api.Data.DTO;
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
    [Route("all")]
    public ActionResult<IEnumerable<List<string>>> GetSchedulers()
    {
        var schedulers = _jobRepository.GetJobs().Select(x => x.SchedName).Distinct();
        return Ok(schedulers);
    }

    [HttpGet]
    public ActionResult<IEnumerable<JobDTO>> GetJobs()
    {
        return Ok(_jobRepository.GetJobs());
    }
}