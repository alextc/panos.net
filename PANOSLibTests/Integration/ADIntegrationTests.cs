namespace PANOSLibTest.Integration
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PANOS;
    using PANOS.Integration;

    [TestClass]
    public class AdIntegrationTests : BaseConfigTest
    {
        [TestMethod]
        public void AddressGroupFromDomainControllersInRootDomainTest()
        {
            var activeDirectoryRepository = new ActiveDirectoryRepository("NTDEV.CORP.MICROSOFT.COM", null);
            var group = activeDirectoryRepository.AddressGroupFromDomainControllersInRootDomain("NTDEVDCs");
            Assert.IsNotNull(group);
            Assert.AreEqual(group.Members.Count, group.MemberObjects.Count);
            foreach (var memberObject in group.MemberObjects)
            {
                Assert.IsTrue(group.Members.Contains(memberObject.Name));
                Assert.IsInstanceOfType(memberObject, typeof(AddressObject));
            }
        }
    }
}
