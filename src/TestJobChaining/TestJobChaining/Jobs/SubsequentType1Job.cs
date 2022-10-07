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
    public class SubsequentType1Job : IJob
    {
        private readonly ILogger<SubsequentType1Job> _logger;

        public SubsequentType1Job(ILogger<SubsequentType1Job> logger)
        {
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"Executing Task SubsequentType1Job. {DateTime.Now}");
            await Task.Delay(5000);

            context.MergedJobDataMap["NextJobName"] = "SubsequentType2Job";
        }
    }
}
