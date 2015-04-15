namespace PANOS.Integration
{
    using System.DirectoryServices.ActiveDirectory;
    using System.Linq;
    using System.Management.Automation;
    using PANOS.Logging;

    public class ActiveDirectoryRepository
    {
        private string ForestName { get; set; }
        private PSCredential Credential { get; set; }
        private readonly DnsRepository dnsRepository = new DnsRepository();

        public ActiveDirectoryRepository(string forestName, PSCredential psCredential)
        {
            ForestName = forestName;
            Credential = psCredential;
        }

        public AddressGroupObject AddressGroupFromDomainControllersInRootDomain(string addressGroupName)
        {
            var context = Credential != null ?
                new DirectoryContext(DirectoryContextType.Forest, ForestName, Credential.UserName, Credential.GetNetworkCredential().Password) :
                new DirectoryContext(DirectoryContextType.Forest, ForestName);

            var forest = Forest.GetForest(context);
            Logger.LogActiveDirectoryForestConnection(forest);
            var domainControllers = forest.RootDomain.DomainControllers;
            Logger.LogDisoveredDomainControllers(domainControllers);

            var domainControllersAddressObjects = domainControllers.Cast<DomainController>().
                Select(domainController => dnsRepository.IpV4AddressObjectFromFqdn(domainController.Name)).
                Where(address => address != null).
                Cast<FirewallObject>().
                ToList();

            var result = new AddressGroupObject(
                addressGroupName,
                domainControllersAddressObjects.Select(d => d.Name).ToList())
                {
                    MemberObjects = domainControllersAddressObjects
                };

            return result;
        }
    }
}
