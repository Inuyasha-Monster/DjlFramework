using System;
using System.Reflection;
using System.Threading.Tasks;
using Djl.EventDbContext.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Djl.EventDbContext.Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();
            services.AddDbContext<MqEventDbcontext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MqEventDbcontext;Trusted_Connection=True;",
                    x => x.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));
            });
            IServiceProvider serviceProvider = services.BuildServiceProvider(true);
            var mqEventDbcontext = serviceProvider.GetRequiredService<MqEventDbcontext>();

            await mqEventDbcontext.Database.EnsureCreatedAsync();

            await mqEventDbcontext.MqEventMessages.AddAsync(new MqEventMessage()
            {
                AssemblyName = "AssemblyName",
                JsonBody = "JsonBody",
                ClassFullName = "ClassFullName"
            });
            await mqEventDbcontext.SaveChangesAsync();

            var mqEventMessages = await mqEventDbcontext.MqEventMessages.ToListAsync();
            foreach (var mqEventMessage in mqEventMessages)
            {
                Console.WriteLine(mqEventMessage.Id);
            }

            Console.ReadKey();
        }
    }
}
