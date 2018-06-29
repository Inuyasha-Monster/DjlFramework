using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Djl.Quartz.Test
{
    public class QuartzUnitTest
    {
        private readonly ITestOutputHelper _output;

        public QuartzUnitTest(ITestOutputHelper testOutputHelper)
        {
            _output = testOutputHelper;
        }

        [Fact]
        public void TestQuartz()
        {
            _output.WriteLine($"开始测试定时调度任务 {DateTime.Now.ToString()}");
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<ITestOutputHelper>(_output);
            services.AddLogging(x => x.AddConsole());
            services.AddQuartz();
            IApplicationBuilder applicationBuilder = new ApplicationBuilder(services.BuildServiceProvider());
            applicationBuilder.UseQuartz();
            Task.Delay(TimeSpan.FromSeconds(12)).Wait();
            _output.WriteLine($"结束测试定时调度任务 {DateTime.Now.ToString()}");
        }
    }
}
