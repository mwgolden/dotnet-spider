using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace scheduler_api;

public partial class QuartznetContext : DbContext
{
    public QuartznetContext()
    {
    }

    public QuartznetContext(DbContextOptions<QuartznetContext> options)
        : base(options)
    {
    }

    public virtual DbSet<QrtzBlobTrigger> QrtzBlobTriggers { get; set; }

    public virtual DbSet<QrtzCalendar> QrtzCalendars { get; set; }

    public virtual DbSet<QrtzCronTrigger> QrtzCronTriggers { get; set; }

    public virtual DbSet<QrtzFiredTrigger> QrtzFiredTriggers { get; set; }

    public virtual DbSet<QrtzJobDetail> QrtzJobDetails { get; set; }

    public virtual DbSet<QrtzLock> QrtzLocks { get; set; }

    public virtual DbSet<QrtzPausedTriggerGrp> QrtzPausedTriggerGrps { get; set; }

    public virtual DbSet<QrtzSchedulerState> QrtzSchedulerStates { get; set; }

    public virtual DbSet<QrtzSimpleTrigger> QrtzSimpleTriggers { get; set; }

    public virtual DbSet<QrtzSimpropTrigger> QrtzSimpropTriggers { get; set; }

    public virtual DbSet<QrtzTrigger> QrtzTriggers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:quartznetdb");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<QrtzBlobTrigger>(entity =>
        {
            entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup }).HasName("qrtz_blob_triggers_pkey");

            entity.ToTable("qrtz_blob_triggers");

            entity.Property(e => e.SchedName)
                .HasMaxLength(120)
                .HasColumnName("sched_name");
            entity.Property(e => e.TriggerName)
                .HasMaxLength(150)
                .HasColumnName("trigger_name");
            entity.Property(e => e.TriggerGroup)
                .HasMaxLength(150)
                .HasColumnName("trigger_group");
            entity.Property(e => e.BlobData).HasColumnName("blob_data");

            entity.HasOne(d => d.QrtzTrigger).WithOne(p => p.QrtzBlobTrigger)
                .HasForeignKey<QrtzBlobTrigger>(d => new { d.SchedName, d.TriggerName, d.TriggerGroup })
                .HasConstraintName("qrtz_blob_triggers_sched_name_trigger_name_trigger_group_fkey");
        });

        modelBuilder.Entity<QrtzCalendar>(entity =>
        {
            entity.HasKey(e => new { e.SchedName, e.CalendarName }).HasName("qrtz_calendars_pkey");

            entity.ToTable("qrtz_calendars");

            entity.Property(e => e.SchedName)
                .HasMaxLength(120)
                .HasColumnName("sched_name");
            entity.Property(e => e.CalendarName)
                .HasMaxLength(200)
                .HasColumnName("calendar_name");
            entity.Property(e => e.Calendar).HasColumnName("calendar");
        });

        modelBuilder.Entity<QrtzCronTrigger>(entity =>
        {
            entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup }).HasName("qrtz_cron_triggers_pkey");

            entity.ToTable("qrtz_cron_triggers");

            entity.Property(e => e.SchedName)
                .HasMaxLength(120)
                .HasColumnName("sched_name");
            entity.Property(e => e.TriggerName)
                .HasMaxLength(150)
                .HasColumnName("trigger_name");
            entity.Property(e => e.TriggerGroup)
                .HasMaxLength(150)
                .HasColumnName("trigger_group");
            entity.Property(e => e.CronExpression)
                .HasMaxLength(250)
                .HasColumnName("cron_expression");
            entity.Property(e => e.TimeZoneId)
                .HasMaxLength(80)
                .HasColumnName("time_zone_id");

            entity.HasOne(d => d.QrtzTrigger).WithOne(p => p.QrtzCronTrigger)
                .HasForeignKey<QrtzCronTrigger>(d => new { d.SchedName, d.TriggerName, d.TriggerGroup })
                .HasConstraintName("qrtz_cron_triggers_sched_name_trigger_name_trigger_group_fkey");
        });

        modelBuilder.Entity<QrtzFiredTrigger>(entity =>
        {
            entity.HasKey(e => new { e.SchedName, e.EntryId }).HasName("qrtz_fired_triggers_pkey");

            entity.ToTable("qrtz_fired_triggers");

            entity.HasIndex(e => e.JobGroup, "idx_qrtz_ft_job_group");

            entity.HasIndex(e => e.JobName, "idx_qrtz_ft_job_name");

            entity.HasIndex(e => e.RequestsRecovery, "idx_qrtz_ft_job_req_recovery");

            entity.HasIndex(e => e.TriggerGroup, "idx_qrtz_ft_trig_group");

            entity.HasIndex(e => e.InstanceName, "idx_qrtz_ft_trig_inst_name");

            entity.HasIndex(e => e.TriggerName, "idx_qrtz_ft_trig_name");

            entity.HasIndex(e => new { e.SchedName, e.TriggerName, e.TriggerGroup }, "idx_qrtz_ft_trig_nm_gp");

            entity.Property(e => e.SchedName)
                .HasMaxLength(120)
                .HasColumnName("sched_name");
            entity.Property(e => e.EntryId)
                .HasMaxLength(140)
                .HasColumnName("entry_id");
            entity.Property(e => e.FiredTime).HasColumnName("fired_time");
            entity.Property(e => e.InstanceName)
                .HasMaxLength(200)
                .HasColumnName("instance_name");
            entity.Property(e => e.IsNonconcurrent).HasColumnName("is_nonconcurrent");
            entity.Property(e => e.JobGroup)
                .HasMaxLength(200)
                .HasColumnName("job_group");
            entity.Property(e => e.JobName)
                .HasMaxLength(200)
                .HasColumnName("job_name");
            entity.Property(e => e.Priority).HasColumnName("priority");
            entity.Property(e => e.RequestsRecovery).HasColumnName("requests_recovery");
            entity.Property(e => e.SchedTime).HasColumnName("sched_time");
            entity.Property(e => e.State)
                .HasMaxLength(16)
                .HasColumnName("state");
            entity.Property(e => e.TriggerGroup)
                .HasMaxLength(150)
                .HasColumnName("trigger_group");
            entity.Property(e => e.TriggerName)
                .HasMaxLength(150)
                .HasColumnName("trigger_name");
        });

        modelBuilder.Entity<QrtzJobDetail>(entity =>
        {
            entity.HasKey(e => new { e.SchedName, e.JobName, e.JobGroup }).HasName("qrtz_job_details_pkey");

            entity.ToTable("qrtz_job_details");

            entity.HasIndex(e => e.RequestsRecovery, "idx_qrtz_j_req_recovery");

            entity.Property(e => e.SchedName)
                .HasMaxLength(120)
                .HasColumnName("sched_name");
            entity.Property(e => e.JobName)
                .HasMaxLength(200)
                .HasColumnName("job_name");
            entity.Property(e => e.JobGroup)
                .HasMaxLength(200)
                .HasColumnName("job_group");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .HasColumnName("description");
            entity.Property(e => e.IsDurable).HasColumnName("is_durable");
            entity.Property(e => e.IsNonconcurrent).HasColumnName("is_nonconcurrent");
            entity.Property(e => e.IsUpdateData).HasColumnName("is_update_data");
            entity.Property(e => e.JobClassName)
                .HasMaxLength(250)
                .HasColumnName("job_class_name");
            entity.Property(e => e.JobData).HasColumnName("job_data");
            entity.Property(e => e.RequestsRecovery).HasColumnName("requests_recovery");
        });

        modelBuilder.Entity<QrtzLock>(entity =>
        {
            entity.HasKey(e => new { e.SchedName, e.LockName }).HasName("qrtz_locks_pkey");

            entity.ToTable("qrtz_locks");

            entity.Property(e => e.SchedName)
                .HasMaxLength(120)
                .HasColumnName("sched_name");
            entity.Property(e => e.LockName)
                .HasMaxLength(40)
                .HasColumnName("lock_name");
        });

        modelBuilder.Entity<QrtzPausedTriggerGrp>(entity =>
        {
            entity.HasKey(e => new { e.SchedName, e.TriggerGroup }).HasName("qrtz_paused_trigger_grps_pkey");

            entity.ToTable("qrtz_paused_trigger_grps");

            entity.Property(e => e.SchedName)
                .HasMaxLength(120)
                .HasColumnName("sched_name");
            entity.Property(e => e.TriggerGroup)
                .HasMaxLength(150)
                .HasColumnName("trigger_group");
        });

        modelBuilder.Entity<QrtzSchedulerState>(entity =>
        {
            entity.HasKey(e => new { e.SchedName, e.InstanceName }).HasName("qrtz_scheduler_state_pkey");

            entity.ToTable("qrtz_scheduler_state");

            entity.Property(e => e.SchedName)
                .HasMaxLength(120)
                .HasColumnName("sched_name");
            entity.Property(e => e.InstanceName)
                .HasMaxLength(200)
                .HasColumnName("instance_name");
            entity.Property(e => e.CheckinInterval).HasColumnName("checkin_interval");
            entity.Property(e => e.LastCheckinTime).HasColumnName("last_checkin_time");
        });

        modelBuilder.Entity<QrtzSimpleTrigger>(entity =>
        {
            entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup }).HasName("qrtz_simple_triggers_pkey");

            entity.ToTable("qrtz_simple_triggers");

            entity.Property(e => e.SchedName)
                .HasMaxLength(120)
                .HasColumnName("sched_name");
            entity.Property(e => e.TriggerName)
                .HasMaxLength(150)
                .HasColumnName("trigger_name");
            entity.Property(e => e.TriggerGroup)
                .HasMaxLength(150)
                .HasColumnName("trigger_group");
            entity.Property(e => e.RepeatCount).HasColumnName("repeat_count");
            entity.Property(e => e.RepeatInterval).HasColumnName("repeat_interval");
            entity.Property(e => e.TimesTriggered).HasColumnName("times_triggered");

            entity.HasOne(d => d.QrtzTrigger).WithOne(p => p.QrtzSimpleTrigger)
                .HasForeignKey<QrtzSimpleTrigger>(d => new { d.SchedName, d.TriggerName, d.TriggerGroup })
                .HasConstraintName("qrtz_simple_triggers_sched_name_trigger_name_trigger_group_fkey");
        });

        modelBuilder.Entity<QrtzSimpropTrigger>(entity =>
        {
            entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup }).HasName("qrtz_simprop_triggers_pkey");

            entity.ToTable("qrtz_simprop_triggers");

            entity.Property(e => e.SchedName)
                .HasMaxLength(120)
                .HasColumnName("sched_name");
            entity.Property(e => e.TriggerName)
                .HasMaxLength(150)
                .HasColumnName("trigger_name");
            entity.Property(e => e.TriggerGroup)
                .HasMaxLength(150)
                .HasColumnName("trigger_group");
            entity.Property(e => e.BoolProp1).HasColumnName("bool_prop_1");
            entity.Property(e => e.BoolProp2).HasColumnName("bool_prop_2");
            entity.Property(e => e.DecProp1).HasColumnName("dec_prop_1");
            entity.Property(e => e.DecProp2).HasColumnName("dec_prop_2");
            entity.Property(e => e.IntProp1).HasColumnName("int_prop_1");
            entity.Property(e => e.IntProp2).HasColumnName("int_prop_2");
            entity.Property(e => e.LongProp1).HasColumnName("long_prop_1");
            entity.Property(e => e.LongProp2).HasColumnName("long_prop_2");
            entity.Property(e => e.StrProp1)
                .HasMaxLength(512)
                .HasColumnName("str_prop_1");
            entity.Property(e => e.StrProp2)
                .HasMaxLength(512)
                .HasColumnName("str_prop_2");
            entity.Property(e => e.StrProp3)
                .HasMaxLength(512)
                .HasColumnName("str_prop_3");
            entity.Property(e => e.TimeZoneId)
                .HasMaxLength(80)
                .HasColumnName("time_zone_id");

            entity.HasOne(d => d.QrtzTrigger).WithOne(p => p.QrtzSimpropTrigger)
                .HasForeignKey<QrtzSimpropTrigger>(d => new { d.SchedName, d.TriggerName, d.TriggerGroup })
                .HasConstraintName("qrtz_simprop_triggers_sched_name_trigger_name_trigger_grou_fkey");
        });

        modelBuilder.Entity<QrtzTrigger>(entity =>
        {
            entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup }).HasName("qrtz_triggers_pkey");

            entity.ToTable("qrtz_triggers");

            entity.HasIndex(e => e.NextFireTime, "idx_qrtz_t_next_fire_time");

            entity.HasIndex(e => new { e.NextFireTime, e.TriggerState }, "idx_qrtz_t_nft_st");

            entity.HasIndex(e => e.TriggerState, "idx_qrtz_t_state");

            entity.Property(e => e.SchedName)
                .HasMaxLength(120)
                .HasColumnName("sched_name");
            entity.Property(e => e.TriggerName)
                .HasMaxLength(150)
                .HasColumnName("trigger_name");
            entity.Property(e => e.TriggerGroup)
                .HasMaxLength(150)
                .HasColumnName("trigger_group");
            entity.Property(e => e.CalendarName)
                .HasMaxLength(200)
                .HasColumnName("calendar_name");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .HasColumnName("description");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.JobData).HasColumnName("job_data");
            entity.Property(e => e.JobGroup)
                .HasMaxLength(200)
                .HasColumnName("job_group");
            entity.Property(e => e.JobName)
                .HasMaxLength(200)
                .HasColumnName("job_name");
            entity.Property(e => e.MisfireInstr).HasColumnName("misfire_instr");
            entity.Property(e => e.NextFireTime).HasColumnName("next_fire_time");
            entity.Property(e => e.PrevFireTime).HasColumnName("prev_fire_time");
            entity.Property(e => e.Priority).HasColumnName("priority");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
            entity.Property(e => e.TriggerState)
                .HasMaxLength(16)
                .HasColumnName("trigger_state");
            entity.Property(e => e.TriggerType)
                .HasMaxLength(8)
                .HasColumnName("trigger_type");

            entity.HasOne(d => d.QrtzJobDetail).WithMany(p => p.QrtzTriggers)
                .HasForeignKey(d => new { d.SchedName, d.JobName, d.JobGroup })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("qrtz_triggers_sched_name_job_name_job_group_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
