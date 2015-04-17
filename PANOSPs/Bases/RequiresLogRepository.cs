namespace PANOS
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;
    using PANOS.Integration;

    public abstract class RequiresLogRepository : PSCmdlet
    {
        private readonly DnsRepository dnsRepository = new DnsRepository();
        private int delay = 3;
        
        [Parameter(Mandatory = true)]
        public Connection[] Connection { get; set; }

        [Parameter]
        public SwitchParameter ResolveHostName { get; set; }

        [Parameter]
        public int Delay
        {
            get
            {
                return delay;
            }
            set
            {
                delay = value;
            }
        }

        protected List<ILogRepository> LogRepositories { get; set; }
        
        protected override void BeginProcessing()
        {
            LogRepositories = new List<ILogRepository>();
            foreach (var connectionProperty in this.Connection)
            {
                LogRepositories.Add(
                    new LogRepository(
                        new LogCommandFactory(
                            new ApiUriFactory(connectionProperty.Host),
                            new LogApiPostKeyValuePairFactory(connectionProperty.AccessToken)))); 
            }
        }

        protected void SendToPipeline(IEnumerable<TrafficLogEntryObject> result)
        {
            if (result == null)
            {
                return;
            }

            var orderedResult = result.OrderByDescending(l => l.ReceiveTime).ToList();
            if (ResolveHostName)
            {
                foreach (var entry in orderedResult)
                {
                    entry.SourceHostName = dnsRepository.HostNameFromIp(entry.Source);
                    entry.DestinationHostName = dnsRepository.HostNameFromIp(entry.Destination);
                    WriteObject(entry);
                }
            }
            else
            {
                WriteObject(orderedResult, true);
            }
        }

        protected void SendToPipelineSimplified(IEnumerable<TrafficLogEntryObject> result)
        {
            if (result == null)
            {
                return;
            }

            var orderedResult = result.OrderByDescending(l => l.ReceiveTime).ToList();
            if (ResolveHostName)
            {
                foreach (var entry in orderedResult)
                {
                    entry.SourceHostName = dnsRepository.HostNameFromIp(entry.Source);
                    entry.DestinationHostName = dnsRepository.HostNameFromIp(entry.Destination);
                    WriteObject(new { entry.ReceiveTime, entry.Action, entry.Destination, entry.App, entry.DestinationHostName});
                }
            }
            else
            {
                WriteObject(orderedResult, true);
            }
        }

        protected void WriteSubResultToVerbose(List<TrafficLogEntryObject> subResult)
        {
            this.WriteVerbose(
                string.Format(
                    "Entries returned:{0}. Last LogId:{1}. Last TimeStamp:{2} FwId:{3}",
                    subResult.Count,
                    subResult.Last().Id,
                    subResult.Last().ReceiveTime,
                    subResult.Last().FwSerialNumber));
        }
    }
}
