namespace PANOS
{
    using System;
    using System.Management.Automation;

    [Cmdlet(VerbsCommon.Rename, "PANOSObject")]
    [OutputType(typeof(string))]
    public class RenamePanosObject : RequiresConfigRepository
    {
        [Parameter(Mandatory = true, ParameterSetName = "Name", ValueFromPipeline = true)]
        [ValidatePattern("^[A-Za-z0-9-_.]+$")]
        public string Name { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Name")]
        [ValidateSet("address", "address-group")]
        public string SchemaName { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Object", ValueFromPipeline = true)]
        public FirewallObject FirewallObject { get; set; }

        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        [Parameter(ParameterSetName = "Object")]
        [Parameter(ParameterSetName = "Name")]
        [ValidatePattern("^[A-Za-z0-9-_.]+$")]
        public string NewName { get; set; }

        protected override void ProcessRecord()
        {
            switch (ParameterSetName)
            {
                case "Name":
                    WriteObject(this.ConfigRepository.Rename(SchemaName, Name, NewName));
                    break;
                case "Object":
                    WriteObject(this.ConfigRepository.Rename(FirewallObject.SchemaName, FirewallObject.Name, NewName));
                    break;
                default:
                    throw new ArgumentException(string.Format("Unexpected ParameterSetName {0}", ParameterSetName));
            }
        }
    }
}
