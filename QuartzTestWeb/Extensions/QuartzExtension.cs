using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Calendar;
using Quartz.Spi;

namespace Quartz
{
    public static class QuartzExtension
    {
        /// <summary>
        /// 启用Quartz框架的Job
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static async Task AddScheduling(this IServiceCollection services, IConfiguration configuration)
        {
            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler scheduler = await sf.GetScheduler();

            var implementTypes = Assembly.GetEntryAssembly().GetTypes().Where(i => i.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IJob)));
            foreach (var implementType in implementTypes)
            {
                var jobDetail = JobBuilder.Create(implementType).WithIdentity(implementType.Name).Build();
                var trigger = TriggerBuilder.Create().WithCronSchedule(configuration[$"Schueduling:{implementType.Name}"]).Build();
                await scheduler.ScheduleJob(jobDetail, trigger);
            }

            await scheduler.Start();

            services.AddSingleton<ISchedulerFactory>(sf);
            services.AddSingleton(scheduler);
        }
    }
}
