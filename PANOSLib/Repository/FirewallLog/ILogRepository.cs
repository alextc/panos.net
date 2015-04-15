namespace PANOS
{
    using System.Collections.Generic;

    public interface ILogRepository
    {
        IEnumerable<List<TrafficLogEntryObject>> GetTrafficLog(string query, bool page, int delay);
    }
}
