using scheduler.api.Data.DTO;

namespace scheduler.api.Data.DAL;

public interface IJobRepository : IDisposable
{
    IEnumerable<JobDTO> GetJobs();
}