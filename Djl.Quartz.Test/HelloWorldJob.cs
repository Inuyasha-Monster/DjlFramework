using System;
using System.Globalization;
using System.Threading.Tasks;
using Djl.Quartz.Core;
using Microsoft.Extensions.Logging;
using Quartz;
using Xunit.Abstractions;

namespace Djl.Quartz.Test
{
    [JobDescription(description: "HelloWorldJob")]
    [IntervalTrigger(description: "HelloWorldTrigger", intervalInSeconds: 5, startNow: false)]
    public class HelloWorldJob : JobBase
    {
        private readonly ILogger<HelloWorldJob> _logger;
        private readonly ITestOutputHelper _testOutputHelper;

        public HelloWorldJob(ILogger<HelloWorldJob> logger, ITestOutputHelper testOutputHelper)
        {
            _logger = logger;
            _testOutputHelper = testOutputHelper;
        }

        protected override ILogger Logger => _logger;

        protected override async Task ExecuteJob(IJobExecutionContext context)
        {
            await Task.Run(() =>
                {
                    _testOutputHelper.WriteLine($"Hello World {DateTime.Now.ToString(CultureInfo.CurrentCulture)}");
                }
            );
        }
    }

}