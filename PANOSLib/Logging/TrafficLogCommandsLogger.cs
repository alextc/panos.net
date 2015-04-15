namespace PANOS.Logging
{
    using System.Diagnostics;

    // EventIds Start at 600
    public partial class Logger
    {
        private const int TraffilLogJobStatusEventId = 600;

        public static void LogTrafficLogRequestStatus(ApiEnqueuedJobRequestStatusResponse apiEnqueuedJobStatusResponse)
        {
            PanosLogger.WriteEntry(
                string.Format(
                    "Traffic log job:{0} completed with the satus of {1}",
                        apiEnqueuedJobStatusResponse.Result.Job.Id,
                        apiEnqueuedJobStatusResponse.Result.Job.Status),
                EventLogEntryType.Information,
                TraffilLogJobStatusEventId,
                CategoryTrafficLogCommands);
        }
    }
}
