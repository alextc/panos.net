namespace PANOS
{
    using System.Linq;
    using System.Management.Automation;
    using PANOS.Integration;
    using PANOS.Logging;

    [Cmdlet(VerbsData.Import, "RootForestDomainControllersToPANOSAddressGroup")]
    public class SyncDomainControllersIpToPanosAddressGroup : RequiresConnection
    {
        private ICommand<ApiEnqueuedResponse> commitCommand; 
       
        private ActiveDirectoryRepository activeDirectoryRepository;

        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        [ValidatePattern("^[A-Za-z0-9-_.]+$")]
        public string AddressGroupName { get; set; }

        [Parameter(Mandatory = true)]
        public string ForestName { get; set; }

        [Parameter]
        public PSCredential Credential { get; set; }

        private ISearchableRepository<AddressGroupObject> addressGroupSearchableRepository;
        private ISearchableRepository<AddressObject> addressSearchableRepository;
        private IAddableRepository addableRepository;
        private IMembershipRepository membershipRepository;

        protected override void BeginProcessing()
        {
           base.BeginProcessing();
            
           var commitCommandFactory = 
               new CommitApiCommandFactory(
                   new ApiUriFactory(this.Connection.Host),
                   new CommitApiPostKeyValuePairFactory(this.Connection.AccessToken));
            commitCommand = commitCommandFactory.CreateCommit(true);

            activeDirectoryRepository = new ActiveDirectoryRepository(ForestName, Credential);

            addressGroupSearchableRepository = new SearchableRepository<AddressGroupObject>(
                new ConfigCommandFactory(
                   new ApiUriFactory(Connection.Host),
                   new ConfigApiPostKeyValuePairFactory(Connection.AccessToken, Connection.Vsys)),
               Schema.AddressGroupSchemaName);

            addressSearchableRepository = new SearchableRepository<AddressObject>(
                new ConfigCommandFactory(
                   new ApiUriFactory(Connection.Host),
                   new ConfigApiPostKeyValuePairFactory(Connection.AccessToken, Connection.Vsys)),
               Schema.AddressSchemaName);

            addableRepository = new AddableRepository(
                new ConfigCommandFactory(
                   new ApiUriFactory(Connection.Host),
                   new ConfigApiPostKeyValuePairFactory(Connection.AccessToken, Connection.Vsys)));

            membershipRepository = new MembershipRepository(
                new ConfigCommandFactory(
                   new ApiUriFactory(Connection.Host),
                   new ConfigApiPostKeyValuePairFactory(Connection.AccessToken, Connection.Vsys)));
        }

        protected override void ProcessRecord()
        {
            var adView = activeDirectoryRepository.AddressGroupFromDomainControllersInRootDomain(AddressGroupName);
            // TODO: Sanity check, ex 0 members returned

            // This will throw an exception if the group does not exist - Is this Ok?
            var fwView = addressGroupSearchableRepository.GetSingle<GetSingleAddressGroupApiResponse>(
                this.AddressGroupName,
                ConfigTypes.Running).
                Single();
            InflateAddressGroupMembers(fwView);

            if (adView.DeepCompare(fwView))
            {
                WriteVerbose("No Drift detected. Exiting");
                return;
            }
            
            UpdateAddresses(adView, fwView);
            // There is a case where only IP addresses of the member changed, but the membership set remains the same
            // in this case no reason to call UpdateGroup --- something TODO:
            UpdateGroup(adView, fwView);
            Commit();
        }

        private void InflateAddressGroupMembers(AddressGroupObject fwView)
        {
            // Firewall Group may not exist
            if (fwView != null)
            {
                membershipRepository.InflateMembers<AddressObject, GetAllAddressesApiResponse>(
                    addressSearchableRepository,
                    fwView,
                    ConfigTypes.Running);
            }
            else
            {
                this.WriteVerbose(
                    string.Format(
                        "Unable to find an Address Group by the name of {0}, new group will be created",
                        this.AddressGroupName));
            }
        }

        private void UpdateAddresses(AddressGroupObject adView, AddressGroupObject fwView)
        {
            var addressObjectsDetla = adView.GetDelta(fwView);
            if (addressObjectsDetla.Count > 0)
            {
                WriteVerbose("Address configuration drifted:");
                foreach (var address in addressObjectsDetla)
                {
                    addableRepository.Add(address);
                    WriteVerbose(string.Format("Updating {0}", address.Name));
                }
            }
        }

        private void UpdateGroup(AddressGroupObject adView, AddressGroupObject fwView)
        {
            // This is a shallow equal compare.
            // Only checking if member lists (strings) are the same, but this is sufficient for this stage, since we already compared/reconciled the underlying AddressObjects
            if (adView.Equals(fwView))
            {
                // Nothing to do - groups are the same
                return;
            }

            if (fwView != null)
            {
                // Group already exist using Edit API
                membershipRepository.SetGroupMembership(adView);
                WriteVerbose(string.Format("Updating {0}", AddressGroupName));
            }
            else
            {
                // Creating group brand new using Set API
                addableRepository.Add(adView);
                WriteVerbose(string.Format("Setting {0}", AddressGroupName));
            }
        }

        private void Commit()
        {
            var commitResult = this.commitCommand.Execute();
            Logger.LogPanosCommitResponse(commitResult);

            this.WriteVerbose(
                string.Format(
                    "Commit Request was submitted to PANOS. Request submission Status: {0}, Commit Job Id: {1}",
                    commitResult.Status,
                    commitResult.Job.Id));
        }
    }
}
