using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestJobChaining.Jobs;

namespace TestJobChaining.JobListeners
{
    public class MyJobListener : IJobListener
    {
        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException,
            CancellationToken cancellationToken = new CancellationToken())
        {
            Console.WriteLine("Job {0} in group {1} was executed", context.JobDetail.Key.Name, context.JobDetail.Key.Group);

            // only run second job if first job was executed successfully
            if (jobException == null)
            {
                // fetching name of the job to be executed sequentially
                string nextJobName = Convert.ToString(context.MergedJobDataMap.GetString("NextJobName"));

                if (!string.IsNullOrEmpty(nextJobName))
                {
                    Console.WriteLine("Next job to be executed :" + nextJobName);
                    IJobDetail job = null;

                    // define a job and tie it to our JobTwo class
                    if (nextJobName == "SubsequentType1Job") // similarly we can write/handle cases for other jobs as well
                    {
                        job = JobBuilder.Create<SubsequentType1Job>()
                            .WithIdentity("SubsequentType1Job", "SubsequentType1Group")
                            .Build();
                    }
                    if (nextJobName == "SubsequentType2Job") // similarly we can write/handle cases for other jobs as well
                    {
                        job = JobBuilder.Create<SubsequentType2Job>()
                            .WithIdentity("SubsequentType2Job", "SubsequentType2Group")
                            .Build();
                    }
                    if (nextJobName == "SubsequentType3Job") // similarly we can write/handle cases for other jobs as well
                    {
                        job = JobBuilder.Create<SubsequentType3Job>()
                            .WithIdentity("SubsequentType3Job", "SubsequentType3Group")
                            .Build();
                    }

                    // create a trigger to run the job now 
                    ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity("SimpleTrigger", "SimpleTriggerGroup")
                        .StartNow()
                        .Build();

                    // finally, schedule the job
                    if (job != null)
                        context.Scheduler.ScheduleJob(job, trigger);
                }
                else
                {
                    Console.WriteLine("No job to be executed sequentially");
                }
            }
            else
            {
                Console.WriteLine("An exception occured while executing job: {0} in group {1} with following details : {2}",
                    context.JobDetail.Key.Name, context.JobDetail.Key.Group, jobException.Message);
            }
        }

       

        public string Name => "MainJobListner";
    }
}
