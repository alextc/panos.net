namespace PANOS
{
    using System.Management.Automation;
    using PANOS.Integration;
    using PANOS.Logging;

    [Cmdlet(VerbsData.Import, "RootForestDomainControllersToPANOSAddressGroup")]
    public class SyncDomainControllersIpToPanosAddressGroup : RequiresConfigRepository
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

        protected override void BeginProcessing()
        {
           base.BeginProcessing();
            
           var commitCommandFactory = 
               new CommitApiCommandFactory(
                   new ApiUriFactory(this.Connection.Host),
                   new CommitApiPostKeyValuePairFactory(this.Connection.AccessToken));
            commitCommand = commitCommandFactory.CreateCommit(true);

            activeDirectoryRepository = new ActiveDirectoryRepository(ForestName, Credential);
        }

        protected override void ProcessRecord()
        {
            var adView = activeDirectoryRepository.AddressGroupFromDomainControllersInRootDomain(AddressGroupName);
            // TODO: Sanity check, ex 0 members returned

            var fwView = (AddressGroupObject)ConfigRepository.GetSingle<GetSingleAddressGroupApiResponse, AddressGroupObject>(
                    Schema.AddressGroupSchemaName,
                    AddressGroupName,
                    ConfigTypes.Running);
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
                this.ConfigRepository.InflateMembers<GetAllAddressesApiResponse, AddressObject>(
                    fwView,
                    Schema.AddressSchemaName,
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
                    var addressUpdateResult = ConfigRepository.Set(address);
                    WriteVerbose(string.Format("Updating {0} - {1}", address.Name, addressUpdateResult.Message));
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
                var groupUpdateResult = ConfigRepository.SetGroupMembership(adView);
                WriteVerbose(string.Format("Updating {0} - {1}", AddressGroupName, groupUpdateResult.Message));
            }
            else
            {
                // Creating group brand new using Set API
                var groupSetResult = ConfigRepository.Set(adView);
                WriteVerbose(string.Format("Setting {0} - {1}", AddressGroupName, groupSetResult.Message));
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
