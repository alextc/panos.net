namespace PANOS
{
    using System.Management.Automation;

    [Cmdlet(VerbsCommon.Remove, "PANOSAddress")]
    public class DeleteAddress : RequiresConfigRepository
    {
        [Parameter(
            ParameterSetName = "Object",
            Mandatory = true,
            ValueFromPipeline = true)]
        public AddressObject[] PanosAddress { get; set; }

        [Parameter(
            ParameterSetName = "Properties", 
            ValueFromPipeline = true)]
        [ValidatePattern("^[A-Za-z0-9-_.]+$")]
        public string[] Name { get; set; }
        
        [Parameter]
        public SwitchParameter PassThru { get; set; }

        protected override void ProcessRecord()
        {
            switch (ParameterSetName)
            {
                case "Properties":
                    foreach (var name in Name)
                    {
                        ConfigRepository.Delete(Schema.AddressSchemaName, name);
                        if (PassThru) WriteObject(name);
                    }
                    break;
                case "Object":
                    foreach (var addressObject in PanosAddress)
                    {
                        ConfigRepository.Delete(Schema.AddressSchemaName, addressObject.Name);
                        if (PassThru) WriteObject(addressObject);
                    }
                    break;
            }
        }
    }
}
