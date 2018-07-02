using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Spi;

namespace Djl.Quartz.Core
{
    /// <summary>
    /// Job执行任务构造工厂(IServiceProvider来动态解析构建job)
    /// </summary>
    public class DefaultJobFactory : IJobFactory
    {
        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<DefaultJobFactory> _logger;
        /// <summary>
        /// 服务容器
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceProvider"></param>
        public DefaultJobFactory(ILogger<DefaultJobFactory> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 构建Job任务实例
        /// </summary>
        /// <param name="bundle"></param>
        /// <param name="scheduler"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 销毁或者清除任务
        /// </summary>
        /// <param name="job"></param>
        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}