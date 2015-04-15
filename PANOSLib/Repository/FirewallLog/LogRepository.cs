namespace PANOS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public class LogRepository : ILogRepository
    {
        private const uint MaxLogEntriesPanosWillReturn = 5000;
        private readonly ILogCommandFactory logCommandFactory;
        private readonly LogQueryFactory logQueryFactory = new LogQueryFactory();
        
        public LogRepository(ILogCommandFactory logCommandFactory)
        {
            this.logCommandFactory = logCommandFactory;
        }

        public IEnumerable<List<TrafficLogEntryObject>> GetTrafficLog(string suppliedQuery, bool page, int delay)
        {
            // Records are retuned in Desc order, i.e. LogIds with higher values at the top
            var logIdWaterMark = ulong.MaxValue;
            List<TrafficLogEntryObject> subResult;
            var updatedQuery = AdjustQuery(suppliedQuery);

            do
            {
                subResult = GetTrafficLogEntries(updatedQuery, MaxLogEntriesPanosWillReturn, 0, delay);
                if (subResult != null)
                {
                    var mark = logIdWaterMark;
                    yield return (subResult.Where(l => l.Id < mark).ToList());
                    logIdWaterMark = subResult.Last().Id;
                    updatedQuery = logQueryFactory.UpdateGetTrafficWithUpperTimeRange(updatedQuery, subResult.Last().ReceiveTime);
                }
                
            } while (
                subResult != null && 
                subResult.Count == MaxLogEntriesPanosWillReturn &&
                page);
        }

        private string AdjustQuery(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return logQueryFactory.CreateGetTrafficWithUpperTimeRange(DateTime.Now);
            }

            if (logQueryFactory.IsUpperTimeInConditionPresent(query))
            {
                throw new ArgumentException("Time queries based on 'in last' condition are not supported; use leq and geq conditions instead.");
            }

            return logQueryFactory.IsUpperTimeRangeConditionPresent(query) ? query : 
                   logQueryFactory.AppendTrafficWithUpperTimeRange(query, DateTime.Now);
        }

        // TODO: add an OUT parameter that will return the jobId. This will allow the caller to discard the log job once the data is consumed
        private List<TrafficLogEntryObject> GetTrafficLogEntries(string query, uint nlogs, uint skip, int delay)
        {
            var request = this.logCommandFactory.CreateRequestForTrafficLog(query, nlogs, skip).Execute().Job;
            Thread.Sleep(new TimeSpan(0, 0, 0, delay));
            var response = logCommandFactory.CreateConsumeTrafficLog(request.Id).Execute();
            if (response.Result.Job.Status.Equals("FIN"))
            {
                return response.PayLoad;
            }

            throw new Exception("Attempted to consume unfinished log job. Consider increasing dealy value");
        }
    }
}
