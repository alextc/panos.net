namespace PANOS
{
    using System.Management.Automation;
    using System.Net;

    // This is copy-and-past from AddAddress; PANOS treats Add and Set almost the same
    // i.e. this Cmdlet could be used to create a new address, and in the same way Add cmdlet could be used to update an IP address
    // I assume that this may not be intuitive to some folks so simply duplicating the logic
    [Cmdlet(VerbsCommon.Set, "PANOSAddress")]
    public class SetAddress : RequiresConnection
    {
        [Parameter(
            ParameterSetName = "Object",
            Mandatory = true,
            ValueFromPipeline = true)]
        public AddressObject[] PanosAddress { get; set; }

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

        private IAddableRepository addableRepository;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            addableRepository = new AddableRepository(
               new ConfigCommandFactory(
                   new ApiUriFactory(Connection.Host),
                   new ConfigPostKeyValuePairFactory(Connection.AccessToken, Connection.Vsys))
               );
        }

        protected override void ProcessRecord()
        {
            switch (ParameterSetName)
            {
                case "Properties":
                    var newAddress = new AddressObject(Name, IPAddress.Parse(IpAddress), Description);
                    addableRepository.Add(newAddress);
                    if (PassThru) WriteObject(newAddress);
                    break;
                case "Object":
                    foreach (var addressObject in PanosAddress)
                    {
                        addableRepository.Add(addressObject);
                        if (PassThru) WriteObject(addressObject);
                    }
                    break;
            }
        }
    }
}
