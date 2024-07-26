using Quartz;
using Quartz.Spi;
using System;

namespace JobSchedular.Job_Factory
{
    public class JobBuilder : IJobFactory
    {
        private readonly IServiceProvider _ServiceProvider;
        public JobBuilder(IServiceProvider ServiceProvider)
        {
            _ServiceProvider = ServiceProvider;
        }
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var jobDetail = bundle.JobDetail;
            return (IJob)_ServiceProvider.GetService(jobDetail.JobType);
            //throw new NotImplementedException();
        }

        public void ReturnJob(IJob job)
        {
            //throw new NotImplementedException();
        }
    }
}
