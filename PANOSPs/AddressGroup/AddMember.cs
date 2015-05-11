namespace PANOS
{
    using System.Management.Automation;

    [Cmdlet(VerbsCommon.Add, "PANOSAddressGroupMember")]
    public class AddMember : RequiresConnection
    {
        private IMembershipRepository membershipRepository;

        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true)]
        [ValidatePattern("^[A-Za-z0-9-_.]+$")]
        public string GroupName { get; set; }

        [Parameter(
           Mandatory = true,
           ValueFromPipeline = true)]
        [ValidatePattern("^[A-Za-z0-9-_.]+$")]
        public string MemberName { get; set; }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            membershipRepository = new MembershipRepository(
               new ConfigMembershipCommandFactory(
                   new ApiUriFactory(Connection.Host),
                   new ConfigMembershipPostKeyValuePairFactory(Connection.AccessToken, Connection.Vsys))
               );
        }

        protected override void ProcessRecord()
        {
            membershipRepository.AddMember(GroupName, Schema.AddressGroupSchemaName, MemberName);
        }
    }
}
