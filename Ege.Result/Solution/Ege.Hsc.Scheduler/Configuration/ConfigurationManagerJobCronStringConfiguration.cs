namespace Ege.Hsc.Scheduler.Configuration
{
    using System;
    using System.Configuration;

    internal class ConfigurationManagerJobCronStringConfiguration : IJobCronStringConfiguration
    {
        public string GetCronString(string jobName)
        {
            if (string.IsNullOrWhiteSpace(jobName))
            {
                throw new ArgumentException("jobName", string.Format("Job name is null or empty"));
            }
            return ConfigurationManager.AppSettings[string.Format("Jobs.{0}.Cron", jobName)];
        }
    }
}