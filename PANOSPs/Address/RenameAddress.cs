namespace PANOS
{
    using System.Management.Automation;

    [Cmdlet(VerbsCommon.Rename, "PANOSAddress")]
    public class RenameAddress : RequiresConnection
    {
        [Parameter(
            ParameterSetName = "Object",
            Mandatory = true,
            ValueFromPipeline = true)]
        public AddressObject PanosAddress { get; set; }

        [Parameter(
            ParameterSetName = "Properties",
            ValueFromPipeline = true)]
        [ValidatePattern("^[A-Za-z0-9-_.]+$")]
        public string Name { get; set; }

        [Parameter(ParameterSetName = "Properties", ValueFromPipeline = true)]
        [Parameter(ParameterSetName = "Object")]
        [ValidatePattern("^[A-Za-z0-9-_.]+$")]
        public string NewName { get; set; }

        [Parameter]
        public SwitchParameter PassThru { get; set; }

        private IRenamableRepository renamableRepository;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            this.renamableRepository = new RenamableRepository(new ConfigCommandFactory(
                   new ApiUriFactory(Connection.Host),
                   new ConfigPostKeyValuePairFactory(Connection.AccessToken, Connection.Vsys)));
        }


        protected override void ProcessRecord()
        {
            switch (ParameterSetName)
            {
                case "Properties":
                    renamableRepository.Rename(Schema.AddressSchemaName, Name, NewName);
                    if (PassThru) WriteObject(NewName);
                    break;
                case "Object":
                    renamableRepository.Rename(Schema.AddressSchemaName, PanosAddress.Name, NewName);
                    if (PassThru)
                    {
                        PanosAddress.Name = NewName;
                        WriteObject(PanosAddress);
                    }
                    break;
            }
        }
    }
}
