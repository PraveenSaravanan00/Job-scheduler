
using JobSchedular.Model;
using Quartz;
using Quartz.Spi;
using System.Threading;
using System.Threading.Tasks;

namespace JobSchedular.Schedular
{
    public class SchedularClass : IHostedService
    {
        public IScheduler Scheduler { get; set; }
        private readonly IJobFactory jobFactory;

        private readonly JobMetadata jobMetadata;

        private readonly ISchedulerFactory schedulerFactory;
        public SchedularClass(ISchedulerFactory schedulerFactory, JobMetadata jobMetadata, IJobFactory jobFactory)
        {
            this.schedulerFactory = schedulerFactory;
            this.jobMetadata = jobMetadata;
            this.jobFactory = jobFactory;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //create the scheduler
            Scheduler = await schedulerFactory.GetScheduler();
            Scheduler.JobFactory = jobFactory;


            //create the job
            IJobDetail jobDetail = CreateJob(jobMetadata);

            //Create the Triggers
            ITrigger trigger = CreateTrigger(jobMetadata);

            await Scheduler.ScheduleJob(jobDetail, trigger, cancellationToken);
            await Scheduler.Start(cancellationToken);
            //throw new NotImplementedException();
        }

        private ITrigger CreateTrigger(JobMetadata jobMetadata)
        {
            return TriggerBuilder.Create()
            .WithIdentity(jobMetadata.JobId.ToString())
            .WithCronSchedule(jobMetadata.CronExpression)
            .WithDescription(jobMetadata.JobName).Build();
            //throw new NotImplementedException();
        }

        private IJobDetail CreateJob(JobMetadata jobMetadata)
        {
            return JobBuilder.Create(jobMetadata.JobType)
                .WithIdentity(jobMetadata.JobId.ToString())
                .WithDescription(jobMetadata.JobName).Build();
            //throw new NotImplementedException();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Scheduler.Shutdown();
            //throw new NotImplementedException();
        }
    }
}
