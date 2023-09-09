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
    public ActionResult<IEnumerable<JobDTO>> GetJobs()
    {
        return Ok(_jobRepository.GetJobs());
    }
}