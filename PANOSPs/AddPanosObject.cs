namespace PANOS
{
    using System.Management.Automation;

    [Cmdlet(VerbsCommon.Add, "PANOSObject")]
    [OutputType(typeof(ApiResponseWithMessage))]
    public class AddPanosObject : RequiresConfigRepository
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public FirewallObject[] FirewallObjects { get; set; }
        
        protected override void ProcessRecord()
        {
            foreach (var firewallObject in FirewallObjects)
            {
                WriteObject(this.ConfigRepository.Set(firewallObject));
            }
        }
    }
}