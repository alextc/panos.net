namespace PANOSLibTest.API.AddressGroup
{
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using PANOS;

    [TestClass]
    public class MemberShipTests : BaseConfigTest
    {
        // TODO: Adding a member that does not exist
        // TODO: Replacing membership with NULL (i.e. no members)

        [TestMethod]
        public void ReplaceMembership()
        {
            // Setup
            // This will create a new group with 3 members: address, range and subnet
            var addressGroupUnderTest = RandomObjectFactory.GenerateRandomObject<AddressGroupObject>();
            Assert.IsNotNull(ConfigRepository.Set(addressGroupUnderTest));
            
            // Remove existing member
            var memberToRemove = addressGroupUnderTest.Members.Last();
            addressGroupUnderTest.Members.Remove(memberToRemove);

            // Add new AddressObject
            var newAddress = RandomObjectFactory.GenerateRandomObject<AddressObject>();
            Assert.IsNotNull(ConfigRepository.Set(newAddress));
            addressGroupUnderTest.Members.Add(newAddress.Name);
            
            // Test
            var groupUpdateResult =
                ConfigRepository.SetGroupMembership(addressGroupUnderTest);

            // Validate
            Assert.IsTrue(groupUpdateResult.Status.Equals("success"));
            var updatedGroup = (AddressGroupObject)ConfigRepository.GetSingle<GetSingleAddressGroupApiResponse, AddressGroupObject>(
                Schema.AddressGroupSchemaName,
                addressGroupUnderTest.Name,
                ConfigTypes.Candidate);

            Assert.AreEqual(addressGroupUnderTest.Members.Count, updatedGroup.Members.Count);
            Assert.IsFalse(updatedGroup.Members.Contains(memberToRemove));
            Assert.IsTrue(updatedGroup.Members.Contains(newAddress.Name));

            // Clean-up
            ConfigRepository.Delete(Schema.AddressGroupSchemaName, addressGroupUnderTest.Name);
            ConfigRepository.Delete(Schema.AddressSchemaName, memberToRemove);
            foreach (var member in updatedGroup.Members)
            {
                ConfigRepository.Delete(Schema.AddressSchemaName, member);
            }
        }

        [TestMethod]
        public void InflateMembersTest()
        {
            // Setup
            // This will create a new group with 3 members: address, range and subnet
            var addressGroupUnderTest = RandomObjectFactory.GenerateRandomObject<AddressGroupObject>();
            Assert.IsNotNull(ConfigRepository.Set(addressGroupUnderTest));
            
            // Test
            ConfigRepository.InflateMembers<GetAllAddressesApiResponse, AddressObject>(
                addressGroupUnderTest,
                Schema.AddressSchemaName,
                ConfigTypes.Candidate);
            ConfigRepository.InflateMembers<GetAllAddressesApiResponse, SubnetObject>(
                addressGroupUnderTest,
                Schema.AddressSchemaName,
                ConfigTypes.Candidate);
            ConfigRepository.InflateMembers<GetAllAddressesApiResponse, AddressRangeObject>(
                addressGroupUnderTest,
                Schema.AddressSchemaName,
                ConfigTypes.Candidate);

            // Validate
            Assert.AreEqual(addressGroupUnderTest.MemberObjects.Count, addressGroupUnderTest.Members.Count);
            foreach (var memberObject in addressGroupUnderTest.MemberObjects)
            {
                Assert.IsTrue(addressGroupUnderTest.Members.Contains(memberObject.Name));
            }
            
            // Clean-up
            ConfigRepository.Delete(Schema.AddressGroupSchemaName, addressGroupUnderTest.Name);
            foreach (var member in addressGroupUnderTest.Members)
            {
                ConfigRepository.Delete(Schema.AddressSchemaName, member);
            }
        }
    }
}
