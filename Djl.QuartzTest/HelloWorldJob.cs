using System;
using System.Threading.Tasks;
using Djl.Quartz;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Djl.QuartzTest
{
    [JobDescription("HelloWorldJob")]
    [IntervalTrigger("HelloWorldTrigger", 5)]
    public class HelloWorldJob : JobBase
    {
        private readonly ILogger<HelloWorldJob> _logger;

        public HelloWorldJob(ILogger<HelloWorldJob> logger)
        {
            _logger = logger;
        }

        protected override ILogger Logger => _logger;

        protected override async Task ExecuteJob(IJobExecutionContext context)
        {
            await Task.Run(() => Console.WriteLine("Hello World"));
        }
    }
}