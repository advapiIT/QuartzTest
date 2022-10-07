using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestJobChaining.Jobs
{
    [DisallowConcurrentExecution]
    public class SubsequentType2Job : IJob
    {
        private readonly ILogger<SubsequentType2Job> _logger;

        public SubsequentType2Job(ILogger<SubsequentType2Job> logger)
        {
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"Executing Task SubsequentType2Job. {DateTime.Now}");
            await Task.Delay(5000);

            context.MergedJobDataMap["NextJobName"] = "SubsequentType3Job";
        }
    }
}
