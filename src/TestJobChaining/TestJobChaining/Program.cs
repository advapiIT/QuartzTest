using CSIOSService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Quartz;
using Serilog;
using TestJobChaining.JobListeners;
using TestJobChaining.Jobs;

namespace TestJobChaining
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()

                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration).Enrich.WithProperty("ApplicationName",
                    configuration.GetValue<string>("ApplicationName"))

                .CreateLogger();

            var host = Host.CreateDefaultBuilder(args).ConfigureServices(services =>
            {
                // Add the required Quartz.NET services
                services.AddQuartz(q =>
                {
                    q.UseMicrosoftDependencyInjectionJobFactory();

                    // Register the job, loading the schedule from configuration
                    q.AddJobAndTrigger<MainJob>(configuration);

                    q.AddJobListener<MyJobListener>();
                    services.AddQuartzHostedService(
                        q => q.WaitForJobsToComplete = true);

                });
            }).UseSerilog().Build();

            host.Run();
        }
    }
}