namespace PANOSLibTest.API.Address
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PANOS;

    [TestClass]
    public class RenameAddressTests : BaseConfigTest
    {
        private readonly RenameTests baseRenameTests = new RenameTests();
        [TestMethod]
        public void RenameAddressTest()
        {
            Assert.IsTrue(baseRenameTests.RenameObject<GetSingleAddressApiResponse, AddressObject>());
            Assert.IsTrue(baseRenameTests.RenameObject<GetSingleAddressApiResponse, SubnetObject>());
            Assert.IsTrue(baseRenameTests.RenameObject<GetSingleAddressApiResponse, AddressRangeObject>());
            Assert.IsTrue(baseRenameTests.RenameObject<GetSingleAddressGroupApiResponse, AddressGroupObject>());
        }

        [TestMethod]
        [ExpectedException(typeof(AttemptToRenameNonExistingObject))]
        public void RenameNonExistingAddressTest()
        {
            baseRenameTests.RenameNonExistingTest<AddressObject>();
        }

        [TestMethod]
        [ExpectedException(typeof(AttemptToRenameNonExistingObject))]
        public void RenameNonExistingSubnetTest()
        {
            baseRenameTests.RenameNonExistingTest<SubnetObject>();
        }

        [TestMethod]
        [ExpectedException(typeof(AttemptToRenameNonExistingObject))]
        public void RenameNonExistingAddressRangeTest()
        {
            baseRenameTests.RenameNonExistingTest<AddressRangeObject>();
        }

        [TestMethod]
        [ExpectedException(typeof(AttemptToRenameNonExistingObject))]
        public void RenameNonExistingAddressGroupTest()
        {
            baseRenameTests.RenameNonExistingTest<AddressGroupObject>();
        }
    }
}
