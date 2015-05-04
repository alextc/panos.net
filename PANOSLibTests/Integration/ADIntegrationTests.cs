namespace PANOSLibTest.Integration
{
    using PANOS;
    using PANOS.Integration;
    using NUnit.Framework;

    [TestFixture]
    public class AdIntegrationTests : BaseConfigTest
    {
        [Test]
        public void AddressGroupFromDomainControllersInRootDomainTest()
        {
            var activeDirectoryRepository = new ActiveDirectoryRepository("NTDEV.CORP.MICROSOFT.COM", null);
            var group = activeDirectoryRepository.AddressGroupFromDomainControllersInRootDomain("NTDEVDCs");
            Assert.IsNotNull(group);
            Assert.AreEqual(group.Members.Count, group.MemberObjects.Count);
            foreach (var memberObject in group.MemberObjects)
            {
                Assert.IsTrue(group.Members.Contains(memberObject.Name));
                Assert.That(memberObject, Is.TypeOf<AddressObject>());
            }
        }
    }
}
