using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Djl.Quartz
{
    /// <summary>
    /// Job基础类防并发以及持久化Job数据
    /// DisallowConcurrentExecution:当任务执行时间大于触发器间隔时间的时候,依然会像你期望的执行方式正确运行
    /// PersistJobDataAfterExecution:在每次任务执行完毕的时候更新持久化任务的JobDataMap数据
    /// </summary>
    [DisallowConcurrentExecution]
    [PersistJobDataAfterExecution]
    public abstract class JobBase : IJob
    {
        /// <summary>
        /// 抽象日志记录器
        /// </summary>
        protected abstract ILogger Logger { get; }

        /// <summary>
        /// 任务执行方法
        /// </summary>
        /// <param name="context">任务执行上下文</param>
        /// <returns></returns>
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                return ExecuteJob(context);
            }
            catch (Exception exception)
            {
                // 获取底层异常
                exception = exception.GetBaseException();
                // 记录异常Error信息
                Logger.LogError(exception, $"当前任务Key:{context.JobDetail.Key};当前任务描述Description:{context.JobDetail.Description},当前任务所属触发器Key:{context.Trigger.Key},当前任务所属触发器描述Description:{context.Trigger.Description},执行出现未知异常");
                // 构建任务执行异常交由调度器重新激活
                JobExecutionException jobExecutionException = new JobExecutionException(exception, refireImmediately: true);
                throw jobExecutionException;
            }
        }

        /// <summary>
        /// 抽象任务执行方法(提供子类实现自定义任务执行逻辑)
        /// </summary>
        /// <param name="context">任务执行上下文</param>
        /// <returns></returns>
        protected abstract Task ExecuteJob(IJobExecutionContext context);
    }
}