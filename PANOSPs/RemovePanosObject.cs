namespace PANOS
{
    using System;
    using System.Management.Automation;

    [Cmdlet(VerbsCommon.Remove, "PANOSObject")]
    [OutputType(typeof(string))]
    public class RemovePanosObject : RequiresConfigRepository
    {
        [Parameter(Mandatory = true, ParameterSetName = "Name", ValueFromPipeline = true)]
        [ValidatePattern("^[A-Za-z0-9-_.]+$")]
        public string[] Name { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Name")]
        [ValidateSet("address", "address-group")]
        public string SchemaName { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Object", ValueFromPipeline = true)]
        public FirewallObject[] FirewallObject { get; set; }

        protected override void ProcessRecord()
        {
            switch (ParameterSetName)
            {
                case "Name":
                    foreach (var name in Name)
                    {
                        WriteObject(this.ConfigRepository.Delete(SchemaName, name));
                    }
                    break;
                case "Object":
                    foreach (var firewallObject in FirewallObject)
                    {
                        WriteObject(this.ConfigRepository.Delete(firewallObject.SchemaName, firewallObject.Name));
                    }
                    break;
                default:
                    throw new ArgumentException(string.Format("Unexpected ParameterSetName {0}", ParameterSetName));
            }
        }
    }
}
