namespace PANOS.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net;
    using System.Text;

    public static partial class Logger
    {
        private const string EventSource = "PANOS. NET";
        private static readonly EventLog PanosLogger;

        #region Categories
        // TODO: Refactor this into Enum

        private const Int16 CategoryLogicFlow = 10;

        private const Int16 CategoryDeserialization = 11;

        private const Int16 CategoryActiveDirectory = 20;

        private const Int16 CategoryDnsResolution = 30;

        private const Int16 CategoryPanosCommunication = 40;

        private const Int16 CategoryPanosConfigCommands = 50;

        private const Int16 CategoryIntegration = 60;

        private const Int16 CategoryDllConfiguration = 70;

        private const Int16 CategoryTrafficLogCommands = 80;

        #endregion

        #region EventId
        
        // Deserialization
        private const int LogDeserializationFailureEventId = 50;

        // DNS Events
        private const int LogDnsResolutionResultEventId = 300;
        private const int LogDnsResolutionFailureEventId = 301;

        // DllConfiguration Events
        private const int LogConfigurationReadEventId = 500;
        
        #endregion

        static Logger()
        {
            PanosLogger = new EventLog { Source = EventSource };
        }

        #region DNS Events
        public static void LogDnsResolutionResult(string name, IPAddress address)
        {
            PanosLogger.WriteEntry(
                string.Format("Resolved {0} to {1}", name, address),
            EventLogEntryType.Information,
            LogDnsResolutionResultEventId,
            CategoryDnsResolution);
        }

        public static void LogDnsResolutionFailure(string name)
        {
            PanosLogger.WriteEntry(
                string.Format("Failed to resolve {0}", name), 
                EventLogEntryType.Error,
                LogDnsResolutionFailureEventId,
                CategoryDnsResolution);
        }

        #endregion
        
        #region Deserealization

        public static void LogDeserializationFailure(Exception ex)
        {
            PanosLogger.WriteEntry(
                string.Format("Failed to Deserialize object:{0}{1}", Environment.NewLine, ex.Message),
                EventLogEntryType.Error,
                LogDeserializationFailureEventId,
                CategoryDeserialization);
        }

        #endregion

        #region DllConfiguration

        public static void LogFirewallConnectionsReadEvent(List<Connection> connectionPropertieses)
        {
            var sb = new StringBuilder();
            foreach (var connectionProperty in connectionPropertieses)
            {
                sb.AppendLine(connectionProperty.ToString());
            }

            PanosLogger.WriteEntry(string.Format("PANOS.dll.config read {0}{1}", Environment.NewLine, sb));
        }

        #endregion
    }
}
