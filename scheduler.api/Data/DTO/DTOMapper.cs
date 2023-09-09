using scheduler.api.Data.Entities;

namespace scheduler.api.Data.DTO;

public class DTOMapper
{
    public static JobDTO ToJobDTOMap(QrtzJobDetail jobDetail)
    {
        return new JobDTO()
        {
            SchedName = jobDetail.SchedName,
            JobName = jobDetail.JobName,
            JobGroup = jobDetail.JobGroup,
            Description = jobDetail.Description,
            JobClassName = jobDetail.JobClassName,
            IsDurable = jobDetail.IsDurable,
            IsNonconcurrent = jobDetail.IsNonconcurrent,
            IsUpdateData = jobDetail.IsUpdateData,
            RequestsRecovery = jobDetail.RequestsRecovery,
            JobData = jobDetail.JobData,
            JobTriggers = jobDetail.QrtzTriggers.Select(t => DTOMapper.ToJobTriggerDTOMap(t)).ToList() 
        };
    }

    public static JobTriggerDTO ToJobTriggerDTOMap(QrtzTrigger trigger)
    {
        return new JobTriggerDTO()
        {
            SchedName = trigger.SchedName,
            TriggerName = trigger.TriggerName,
            TriggerGroup = trigger.TriggerGroup,
            JobName = trigger.JobName,
            JobGroup = trigger.JobGroup,
            Description = trigger.Description,
            NextFireTime = trigger.NextFireTime,
            PrevFireTime = trigger.PrevFireTime,
            Priority = trigger.Priority,
            TriggerState = trigger.TriggerState,
            TriggerType = trigger.TriggerType,
            StartTime = trigger.StartTime,
            EndTime = trigger.EndTime,
            CalendarName = trigger.CalendarName,
            MisfireInstr = trigger.MisfireInstr,
            JobData = trigger.JobData
        };
    }
}