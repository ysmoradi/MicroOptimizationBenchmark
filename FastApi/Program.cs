using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FastApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
#if DEBUG
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("*****To achieve accurate results, set project configuration to release mode.*****");
            return;
#endif

            await CreateHostBuilder(args)
                .Build()
                .RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(loggingBuilder => loggingBuilder.ClearProviders())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
