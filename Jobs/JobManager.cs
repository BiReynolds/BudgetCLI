using Microsoft.Data.Sqlite;
using BudgetCLI.Data;
using BudgetCLI.Exceptions;
using BudgetCLI.Session;
using BudgetCLI.Jobs.JobInstances;
using BudgetCLI.Data.Models;

namespace BudgetCLI.Jobs
{
    public class JobManager
    {
        static Dictionary<string, IBudgetJob> JobDictionary = new()
        {
            { "AddNewRecurringBillInstances", new AddNewRecurringBillInstancesJob() },
        };
        SqliteConnection Connection;
        SessionManager Session;
        public JobManager(SessionManager session)
        {
            // Console.WriteLine("Getting db connection");
            Connection = DatabaseHelper.GetReadWriteConnection();
            Session = session;
        }

        public void HandleJobs()
        {
            // Console.WriteLine("Opening connection");
            Connection.Open();
            // Console.WriteLine("Connection open");
            // Console.WriteLine("Getting needed jobs");
            List<BudgetJobModel> neededJobs = GetNeededJobs();
            // Console.WriteLine($"Running {neededJobs.Count} jobs");
            foreach (BudgetJobModel job in neededJobs)
            {
                JobDictionary[job.JobName].RunJob(Session);
                DatabaseHelper.MarkJobComplete(job.JobName, Connection);
            }
            // Console.WriteLine("All jobs run successfully");
            Connection.Close();
        }

        List<BudgetJobModel> GetNeededJobs()
        {
            List<BudgetJobModel> allJobs = DatabaseHelper.GetAllJobsFromDatabase(Connection);
            List<BudgetJobModel> result = new();
            foreach (BudgetJobModel jobModel in allJobs)
            {
                if (JobDictionary.ContainsKey(jobModel.JobName))
                {
                    IBudgetJob job = JobDictionary[jobModel.JobName];
                    if (jobModel.LastRunDate == null || job.CheckDue((DateOnly)jobModel.LastRunDate))
                    {
                        result.Add(jobModel);
                    }
                }
                else
                {
                    Console.WriteLine($"When checking jobs, encountered unknown JobName: {jobModel.JobName}");
                }
            }
            return result;
        }

    }
}