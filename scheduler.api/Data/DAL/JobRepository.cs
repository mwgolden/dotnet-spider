using scheduler.api.Data.Context;
using scheduler.api.Data.DTO;

namespace scheduler.api.Data.DAL;

public class JobRepository: IJobRepository, IDisposable
{
    private readonly QuartznetContext _context;
    private bool _disposed = false;

    public JobRepository(QuartznetContext context)
    {
        _context = context;
    }
    public IEnumerable<JobDTO> GetJobs()
    {
       var jobs = _context.QrtzJobDetails.ToList();
        foreach (var job in jobs)
        {
            var triggers = _context.QrtzTriggers.Where(
                t => t.JobName == job.JobName
                     && t.SchedName == job.SchedName
                     && t.JobGroup == job.JobGroup
            ).ToList();
            job.QrtzTriggers = triggers;
        }

        var jobDTOList = jobs.Select(j => DTOMapper.ToJobDTOMap(j)).ToList();

        return jobDTOList;
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!this._disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        this._disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}