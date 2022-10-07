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
    public class SubsequentType3Job : IJob
    {
        private readonly ILogger<SubsequentType3Job> _logger;

        public SubsequentType3Job(ILogger<SubsequentType3Job> logger)
        {
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"Executing Task SubsequentType3Job. {DateTime.Now}");
            await Task.Delay(5000);
        }
    }
}
