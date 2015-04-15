namespace PANOS
{
    using System.Management.Automation;

    [Cmdlet(VerbsCommon.New, "PANOSAddressGroup")]
    [OutputType(typeof(AddressGroupObject))]
    
    public class NewAddressGroup : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true)]
        [ValidatePattern("^[A-Za-z0-9-_.]+$")]
        public string Name { get; set; }

        [Parameter(
           Mandatory = true,
           ValueFromPipeline = true)]
        [ValidatePattern("^[A-Za-z0-9-_.]+$")]
        public string[] Members { get; set; }

        
        protected override void BeginProcessing()
        {
           // TODO: Add Validation

           WriteObject(new AddressGroupObject(Name, Members));
        }
    }
}
