namespace PANOS.Logging
{
    using System;
    using System.Diagnostics;

    public static partial class Logger
    {
        // Logic Flow Events
        private const int LogEnterFunctionEventId = 10;
        private const int LogExitFunctionEventId = 11;
        private const int LogExceptionEventId = 12;

        public static void LogFunctionEntered(string name)
        {
            PanosLogger.WriteEntry(
                string.Format("Entered into Function {0}", name),
                EventLogEntryType.Information,
                LogEnterFunctionEventId,
                CategoryLogicFlow);
        }

        public static void LogFunctionExited(string name)
        {
            PanosLogger.WriteEntry(
                string.Format("Exited Function {0}", name),
                EventLogEntryType.Information,
                LogExitFunctionEventId,
                CategoryLogicFlow);
        }

        public static void LogException(Exception ex, string functionName)
        {
            PanosLogger.WriteEntry(
                string.Format("Exception thrown by {0} {1}", functionName, ex),
                EventLogEntryType.Error,
                LogExceptionEventId,
                CategoryLogicFlow);
        }
    }
}
