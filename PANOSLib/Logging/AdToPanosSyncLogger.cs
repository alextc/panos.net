namespace PANOS.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.DirectoryServices.ActiveDirectory;
    using System.Text;

    public static partial class Logger
    {
        private const int LogActiveDirectoryForestConnectionEventId = 200;
        private const int LogDisoveredDomainControllersEventId = 201;

        private const int LogDcToFwDeltaEventId = 400;
        private const int NoDriftDetectedEventId = 401;

        public static void LogActiveDirectoryForestConnection(Forest forest)
        {
            PanosLogger.WriteEntry(
                string.Format("Established connection with AD Forest {0}",
                    forest.Name),
                EventLogEntryType.Information,
                LogActiveDirectoryForestConnectionEventId,
                CategoryActiveDirectory);
        }

        public static void LogDisoveredDomainControllers(DomainControllerCollection domainControllers)
        {
            var sb = new StringBuilder();
            foreach (DomainController dc in domainControllers)
            {
                sb.AppendFormat("{0}{1}", dc.Name.ToUpper(), Environment.NewLine);
            }

            PanosLogger.WriteEntry(
                string.Format("Discovered following Domain Controllers:{0}{1}",
                Environment.NewLine,
                sb),
                EventLogEntryType.Information,
                LogDisoveredDomainControllersEventId,
                CategoryActiveDirectory);
        }
        
        public static void LogAdToFwDelta(List<AddressObject> delta)
        {
            if (delta == null || delta.Count == 0)
            {
                PanosLogger.WriteEntry(
                    "No Drift was detected between Domain Controllers and Firewall DC's Address Group",
                    EventLogEntryType.Information,
                    NoDriftDetectedEventId,
                    CategoryIntegration);
                return;
            }

            var sb = new StringBuilder();
            foreach (var address in delta)
            {
                sb.AppendFormat("Delta: {0}{1}", address, Environment.NewLine);
            }

            PanosLogger.WriteEntry(
                string.Format("The following DCs are out of sync: {0}", sb),
                EventLogEntryType.Information,
                LogDcToFwDeltaEventId,
                CategoryIntegration);
        }
    }
}
