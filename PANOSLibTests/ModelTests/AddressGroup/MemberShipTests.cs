namespace PANOSLibTest
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
            var addressGroupUnderTest = this.RandomObjectFactory.GenerateRandomObject<AddressGroupObject>();
            this.ConfigRepository.Set(addressGroupUnderTest);
            
            // Remove existing member
            var memberToRemove = addressGroupUnderTest.Members.Last();
            addressGroupUnderTest.Members.Remove(memberToRemove);

            // Add new AddressObject
            var newAddress = this.RandomObjectFactory.GenerateRandomObject<AddressObject>();
            this.ConfigRepository.Set(newAddress);
            addressGroupUnderTest.Members.Add(newAddress.Name);
            
            // Test
            var groupUpdateResult =
                this.ConfigRepository.SetGroupMembership(addressGroupUnderTest);

            // Validate
            Assert.IsTrue(groupUpdateResult.Status.Equals("success"));
            var updatedGroup = this.ConfigRepository.GetSingle<GetSingleAddressGroupApiResponse, AddressGroupObject>(
                Schema.AddressGroupSchemaName,
                addressGroupUnderTest.Name,
                ConfigTypes.Candidate).Single();

            Assert.AreEqual(addressGroupUnderTest.Members.Count, updatedGroup.Members.Count);
            Assert.IsFalse(updatedGroup.Members.Contains(memberToRemove));
            Assert.IsTrue(updatedGroup.Members.Contains(newAddress.Name));

            // Clean-up
            this.ConfigRepository.Delete(Schema.AddressGroupSchemaName, addressGroupUnderTest.Name);
            this.ConfigRepository.Delete(Schema.AddressSchemaName, memberToRemove);
            foreach (var member in updatedGroup.Members)
            {
                this.ConfigRepository.Delete(Schema.AddressSchemaName, member);
            }
        }

        [TestMethod]
        public void InflateMembersTest()
        {
            // Setup
            // This will create a new group with 3 members: address, range and subnet
            var addressGroupUnderTest = this.RandomObjectFactory.GenerateRandomObject<AddressGroupObject>();
            this.ConfigRepository.Set(addressGroupUnderTest);
            
            // Test
            this.ConfigRepository.InflateMembers<GetAllAddressesApiResponse, AddressObject>(
                addressGroupUnderTest,
                Schema.AddressSchemaName,
                ConfigTypes.Candidate);
            this.ConfigRepository.InflateMembers<GetAllAddressesApiResponse, SubnetObject>(
                addressGroupUnderTest,
                Schema.AddressSchemaName,
                ConfigTypes.Candidate);
            this.ConfigRepository.InflateMembers<GetAllAddressesApiResponse, AddressRangeObject>(
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
            this.ConfigRepository.Delete(Schema.AddressGroupSchemaName, addressGroupUnderTest.Name);
            foreach (var member in addressGroupUnderTest.Members)
            {
                this.ConfigRepository.Delete(Schema.AddressSchemaName, member);
            }
        }
    }
}
