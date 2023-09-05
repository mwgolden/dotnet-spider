using message_bus.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace scheduler_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchedulerController : Controller
{
    private readonly IMessageBus _messageBus;
    private readonly QuartznetContext _ctx;

    public SchedulerController(IMessageBus messageBus, QuartznetContext ctx)
    {
        _ctx = ctx;
        _messageBus = messageBus;
    }

    [HttpGet]
    [Route("jobs")]
    public ActionResult<IEnumerable<QrtzJobDetail>> GetJobs()
    {
        return Ok(_ctx.QrtzTriggers);
    }
    [HttpPost]
    public IActionResult Post([FromBody] TestMessage message)
    {
        _messageBus.Publish(message);
        return Ok(message);
    }
}