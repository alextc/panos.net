namespace PANOS
{
    using System.Management.Automation;
    using System.Net;

    [Cmdlet(VerbsCommon.Add, "PANOSAddress")]
    public class AddAddress : RequiresConfigRepository
    {
        [Parameter(
            ParameterSetName = "Object",
            Mandatory = true,
            ValueFromPipeline = true)]
        public AddressObject[] AddressObject { get; set; }

        [Parameter(
            ParameterSetName = "Properties")]
        [ValidatePattern("^[A-Za-z0-9-_.]+$")]
        public string Name { get; set; }

        [Parameter(
            ParameterSetName = "Properties")]
        [ValidatePattern("^[A-Za-z0-9-_. ]+$")]
        public string Description { get; set; }

        // TODO: Add Regex
        [Parameter(
            ParameterSetName = "Properties",
            Mandatory = true)]
        public string IpAddress { get; set; }

        [Parameter]
        public SwitchParameter PassThru { get; set; }

        protected override void ProcessRecord()
        {
            switch (ParameterSetName)
            {
                case "Properties":
                    var newAddress = new AddressObject(Name, IPAddress.Parse(IpAddress), Description);
                    ConfigRepository.Set(newAddress);
                    if (PassThru) WriteObject(newAddress);
                    break;
                case "Object":
                    foreach (var addressObject in AddressObject)
                    {
                        ConfigRepository.Set(addressObject);
                        if (PassThru) WriteObject(addressObject);
                    }
                    break;
            }
        }
    }
}
