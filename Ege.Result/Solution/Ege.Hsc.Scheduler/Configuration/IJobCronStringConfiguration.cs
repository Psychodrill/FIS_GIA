namespace Ege.Hsc.Scheduler.Configuration
{
    public interface IJobCronStringConfiguration
    {
        string GetCronString(string jobName);
    }
}