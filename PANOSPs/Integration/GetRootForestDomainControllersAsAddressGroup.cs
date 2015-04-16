namespace PANOS
{
    using System.Management.Automation;
    using PANOS.Integration;

    [Cmdlet(VerbsCommon.Get, "RootForestDomainControllersAsAddressGroup")]
    [OutputType(typeof(AddressGroupObject))]
    public class GetForestDc : PSCmdlet
    {
        private string forestName;
        private PSCredential credential;
        private ActiveDirectoryRepository activeDirectoryRepository;

        [Parameter(Mandatory = true)]
        public string ForestName
        {
            get { return forestName; }
            set { forestName = value; }
        }

        [Parameter]
        public PSCredential Credential
        {
            get { return credential; }
            set { credential = value;  }
        }

        protected override void BeginProcessing()
        {
            activeDirectoryRepository = new ActiveDirectoryRepository(ForestName, Credential);
        }

        protected override void ProcessRecord()
        {
            WriteObject(activeDirectoryRepository.AddressGroupFromDomainControllersInRootDomain(ForestName));  
        }
    }
}
