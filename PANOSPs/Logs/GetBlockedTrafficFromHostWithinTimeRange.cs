namespace PANOS
{
    using System;
    using System.Management.Automation;
    using System.Net;

    [Cmdlet(VerbsCommon.Get, "PANOSBlockedTraffic")]
    [OutputType(typeof(TrafficLogEntryObject))]
    public class GetBlockedTrafficFromHostWithinTimeRange : RequiresLogRepository
    {
        private string sourceIp;
        private DateTime rangeStart = DateTime.Now.AddHours(-4);
        private DateTime rangeEnd = DateTime.Now;

        [Parameter]
        public string Query { get; set; }

        [Parameter(Mandatory = true)]
        public string SourceIp
        {
            get
            {
                return sourceIp;
            }

            set
            {
                sourceIp = value;
            }
        }

        [Parameter]
        public DateTime RangeStart
        {
            get
            {
                return rangeStart;
            }

            set
            {
                rangeStart = value;
            }
        }

        [Parameter]
        public DateTime RangeEnd
        {
            get
            {
                return rangeEnd;
            }

            set
            {
                rangeEnd = value;
            }
        }

        protected override void ProcessRecord()
        {
            var logQueryFactory = new LogQueryFactory();
            Query = logQueryFactory.CreateGetBlockedTrafficFromSourceWithinTimeRange(
                IPAddress.Parse(SourceIp),
                RangeStart,
                RangeEnd);
            WriteVerbose(string.Format("Log will be restricted to traffic from {0}", SourceIp));

            foreach (var logRepository in LogRepositories)
            {
                foreach (var subResult in logRepository.GetTrafficLog(Query, false, Delay))
                {
                    WriteSubResultToVerbose(subResult);
                    SendToPipelineSimplified(subResult);
                }
            }
        }
    }
}
