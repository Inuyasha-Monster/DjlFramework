using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Spi;

namespace Djl.Quartz
{
    /// <summary>
    /// Job执行任务构造工厂
    /// </summary>
    public class DefaultJobFactory : IJobFactory
    {
        private readonly ILogger<DefaultJobFactory> _logger;
        private readonly IServiceProvider _serviceProvider;

        public DefaultJobFactory(ILogger<DefaultJobFactory> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                var job = this._serviceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
                if (job == null)
                    throw new ArgumentNullException($"从容器IServiceProvider中构建Job:{bundle.JobDetail.JobType.FullName}失败", $"{nameof(job)}");
                return job;
            }
            catch (Exception exception)
            {
                exception = exception.GetBaseException();
                _logger.LogError(exception, $"从容器中构建JobKey:{bundle.JobDetail.Key},JobType:{bundle.JobDetail.JobType.FullName},出现未知异常,请检查是否注入此类型");
                throw new SchedulerException($"Problem while instantiating job '{bundle.JobDetail.Key}' from the DefaultJobFactory.", exception);
            }
        }

        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}