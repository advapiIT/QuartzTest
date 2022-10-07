using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;

namespace TestJobChaining.Jobs
{
    [DisallowConcurrentExecution]
    public class MainJob:IJob
    {
        private readonly ILogger<MainJob> _logger;

        public MainJob(ILogger<MainJob> logger)
        {
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"Executing Task MainJob. {DateTime.Now}");
            await Task.Delay(10000);


            context.MergedJobDataMap["NextJobName"] = "SubsequentType1Job";
        }
    }
}
