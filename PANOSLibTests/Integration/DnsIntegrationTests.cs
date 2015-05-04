namespace PANOSLibTest.Integration
{
    using System.Net;
    using NUnit.Framework;
    using PANOS.Integration;

    [TestFixture]
    public class DnsIntegrationTests
    {
        [Test]
        public void ResolveHostNameFromIpTest()
        {
            var dnsRepository = new DnsRepository();
            var hostName = dnsRepository.HostNameFromIp(IPAddress.Parse("10.221.24.4"));
            Assert.AreEqual(hostName, "db3-red-dc-04.redmond.corp.microsoft.com");
        }

        [Test]
        public void ResolveHostNameFromIpAndDnsQueryFailsTest()
        {
            var dnsRepository = new DnsRepository();
            var hostName = dnsRepository.HostNameFromIp(IPAddress.Parse("1.0.0.1"));
            Assert.AreEqual(hostName, "1.0.0.1");
        }
    }
}
