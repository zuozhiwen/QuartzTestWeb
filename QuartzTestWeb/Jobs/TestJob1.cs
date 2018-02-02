using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;

namespace QuartzTestWeb.Jobs
{
    public class TestJob1 : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            System.Diagnostics.Debug.WriteLine($"{this.GetType().Name} executing...");
        }
    }
}
