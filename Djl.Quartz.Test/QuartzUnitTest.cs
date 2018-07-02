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
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<ITestOutputHelper>(_output);
            services.AddLogging(x => x.AddConsole());
            services.AddQuartz();
            IApplicationBuilder applicationBuilder = new ApplicationBuilder(services.BuildServiceProvider());
            applicationBuilder.UseQuartz();
            Task.Delay(TimeSpan.FromSeconds(15)).Wait();
        }
    }
}
