namespace PANOS
{
    using System.Management.Automation;

    [Cmdlet(VerbsCommon.Get, "PANOSTrafficLog")]
    [OutputType(typeof(TrafficLogEntryObject))]
    public class GetTrafficLog : RequiresLogRepository
    {
        [Parameter]
        public string Query { get; set; }

        [Parameter]
        public SwitchParameter Page { get; set; }

        protected override void ProcessRecord()
        {
            foreach (var logRepository in LogRepositories)
            {
                foreach (var subResult in logRepository.GetTrafficLog(Query, Page, Delay))
                {
                    WriteSubResultToVerbose(subResult);   
                    SendToPipeline(subResult);
                }
            }
        }
    }
}