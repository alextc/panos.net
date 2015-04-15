namespace PANOSLibTest.Integration
{
    using System.Net;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PANOS.Integration;

    [TestClass]
    public class DnsIntegrationTests
    {
        [TestMethod]
        public void ResolveHostNameFromIpTest()
        {
            var dnsRepository = new DnsRepository();
            var hostName = dnsRepository.HostNameFromIp(IPAddress.Parse("10.221.24.4"));
            Assert.AreEqual(hostName, "db3-red-dc-04.redmond.corp.microsoft.com");
        }

        [TestMethod]
        public void ResolveHostNameFromIpAndDnsQueryFailsTest()
        {
            var dnsRepository = new DnsRepository();
            var hostName = dnsRepository.HostNameFromIp(IPAddress.Parse("1.0.0.1"));
            Assert.AreEqual(hostName, "1.0.0.1");
        }
    }
}
