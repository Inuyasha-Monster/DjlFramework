using System;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using Djl.Quartz.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Djl.Quartz
{
    /// <summary>
    /// 定时器框架扩展类
    /// </summary>
    public static class QuartzExtension
    {
        private static void InternalInitScheduer(IServiceCollection service, NameValueCollection props)
        {
            StdSchedulerFactory factory = new StdSchedulerFactory(props);

            // registe scheduerfactory
            service.AddSingleton<ISchedulerFactory>(factory);

            // get a scheduler
            IScheduler scheduler = factory.GetScheduler().Result;

            // register cutomer jobfacotry
            service.AddSingleton<IJobFactory, DefaultJobFactory>(provider => new DefaultJobFactory(provider.GetService<ILogger<DefaultJobFactory>>(), provider));

            // registe all jobs to Iservicecollection
            var jobs = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(x => x.BaseType == typeof(JobBase) || x.GetInterfaces().Any(i => i == typeof(IJob))).Where(x => x.GetCustomAttribute<IgnoreJobAttribute>() == null)
                .Where(x => x.IsAbstract == false);

            foreach (var job in jobs)
            {
                service.AddTransient(job);
                service.AddTransient(typeof(IJob), job);
            }

            // registe scheduer
            service.AddSingleton<IScheduler>(scheduler);
        }

        /// <summary>
        /// 添加Quartz定时器框架扩展
        /// </summary>
        /// <param name="service"></param>
        /// <param name="props"></param>
        public static void AddQuartz(this IServiceCollection service, NameValueCollection props)
        {
            InternalInitScheduer(service, props);
        }

        /// <summary>
        /// 使用默认配置-添加Quartz定时器框架扩展
        /// </summary>
        /// <param name="service"></param>
        public static void AddQuartz(this IServiceCollection service)
        {
            // construct a scheduler factory
            NameValueCollection props = new NameValueCollection
            {
                { "quartz.serializer.type", "json" },
                { "quartz.scheduler.instanceName", "DefaultScheduler" },
                { "quartz.jobStore.type", "Quartz.Simpl.RAMJobStore, Quartz" },
                { "quartz.threadPool.threadCount", "20" }
            };
            InternalInitScheduer(service, props);
        }

        /// <summary>
        /// 启动定时器框架-自动扫描注入启动
        /// </summary>
        /// <param name="app"></param>
        public static void UseQuartz(this IApplicationBuilder app)
        {
            var serviceProvider = app.ApplicationServices;
            var scheduler = serviceProvider.GetRequiredService<IScheduler>();
            var jobFactory = serviceProvider.GetRequiredService<IJobFactory>();
            scheduler.JobFactory = jobFactory;
         

            // add jobdetail with trigger to scheduler 
            var jobs = app.ApplicationServices.GetServices<IJob>();

            foreach (var job in jobs)
            {
                var jobDesc = job.GetType().GetCustomAttribute<JobDescriptionAttribute>();
                var jobBuilder = JobBuilder.Create(job.GetType());
                if (jobDesc != null)
                {
                    jobBuilder.WithIdentity(jobDesc.Key, jobDesc.Group).WithDescription(jobDesc.Description);
                }
                var jobDetail = jobBuilder.Build();

                ITrigger trigger;
                var triggerDesc = job.GetType().GetCustomAttribute<IntervalTriggerAttribute>();
                if (triggerDesc == null)
                {
                    // default trigger
                    trigger = TriggerBuilder.Create().StartNow()
                        .WithSimpleSchedule(x => x.WithIntervalInSeconds(60).RepeatForever()).Build();
                }
                else
                {
                    TriggerBuilder temp = TriggerBuilder.Create().WithIdentity(triggerDesc.Key, triggerDesc.Group).WithDescription(triggerDesc.Description);
                    if (triggerDesc.StartNow)
                    {
                        temp = temp.StartNow();
                    }
                    if (triggerDesc.IsRepeatForever)
                    {
                        temp = temp.WithSimpleSchedule(x => x.WithIntervalInSeconds(triggerDesc.IntervalInSeconds)
                            .RepeatForever());
                    }
                    else
                    {
                        temp = temp.WithSimpleSchedule(x => x.WithIntervalInSeconds(triggerDesc.IntervalInSeconds)
                            .WithRepeatCount(triggerDesc.RepeatCount));
                    }
                    trigger = temp.Build();
                }
                scheduler.ScheduleJob(jobDetail, trigger).Wait();
            }
            scheduler.Start().Wait();
        }
    }
}