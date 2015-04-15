namespace PANOS.Logging
{
    using System;
    using System.Diagnostics;
    using System.Net.Http;

    public static partial class Logger
    {
        private const int LogPanosResponseStatusEventId = 100;
        private const int LogPanosHttpResponseEventId = 101;
        private const int LogPanosAddEditResponseEventId = 102;
        private const int LogPanosFailedAddEditResponseEventId = 103;
        private const int LogPanosConnectionFailureEventId = 104;
        private const int LogPanosCommitRequestSubmittedEventId = 105;

        public static void LogPanosResponseStatus(HttpResponseMessage httpResponseMessage)
        {
            PanosLogger.WriteEntry(
                string.Format("PANOS Responed to {0} with Status Code of {1}",
                    httpResponseMessage.RequestMessage,
                    httpResponseMessage.StatusCode),
                EventLogEntryType.Information,
                LogPanosResponseStatusEventId,
                CategoryPanosCommunication);
        }

        // Not using this one at the moment due to the size limitations of Windows App Log, this is due to the httpResponseMessage might be quite large
        public static void LogPanosHttpResponse(string httpResponseMessage)
        {
            PanosLogger.WriteEntry(
                string.Format("PANOS Responed with: {0}{1}", Environment.NewLine, httpResponseMessage),
            EventLogEntryType.Information,
            LogPanosHttpResponseEventId,
            CategoryPanosCommunication);
        }

        public static void LogPanosConnectionFailure(Exception ex)
        {
            PanosLogger.WriteEntry(
                string.Format("Connection to PANOS Management Interface failed.{0}{1}", Environment.NewLine, ex.Message),
                EventLogEntryType.Error,
                LogPanosConnectionFailureEventId,
                CategoryPanosCommunication);
        }

        public static void LogPanosAddEditResponse(ApiResponseWithMessage apiResponseWithMessage)
        {
            PanosLogger.WriteEntry(
                string.Format(@"PANOS Processed Add/Edit Command.
                               Status: {0}
                               Message: {1}
                               Object Acted Upon: {2} ",
                    apiResponseWithMessage.Status,
                    apiResponseWithMessage.Message,
                    apiResponseWithMessage.ObjectActedUpon),
                 EventLogEntryType.Information,
                 LogPanosAddEditResponseEventId,
                 CategoryPanosConfigCommands);
        }

        public static void LogPanosFailedAddEditResponse(ApiResponseWithMessage apiResponseWithMessage, FirewallObject firewallObject)
        {
            PanosLogger.WriteEntry(
                string.Format(@"PANOS Failed to Process Add/Edit Command.
                               Status: {0}
                               Message: {1}
                               Object Acted Upon: {2} ",
                    apiResponseWithMessage.Status,
                    apiResponseWithMessage.Message,
                    firewallObject),
                 EventLogEntryType.Error,
                 LogPanosFailedAddEditResponseEventId,
                 CategoryPanosConfigCommands);
        }

        public static void LogPanosCommitResponse(ApiEnqueuedResponse apiEnqueuedResponse)
        {
            PanosLogger.WriteEntry(
                string.Format(@"Commit Request was submitted to PANOS.
                               Request submission Status: {0}
                               Commit Job Id: {1}",
                    apiEnqueuedResponse.Status,
                    apiEnqueuedResponse.Job.Id),
                    EventLogEntryType.Information,
                    LogPanosCommitRequestSubmittedEventId,
                    CategoryPanosConfigCommands);
        }
    }
}
