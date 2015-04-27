namespace PANOSLibTest
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
            Assert.IsTrue(this.baseRenameTests.RenameObject<GetSingleAddressApiResponse, AddressObject>());
            Assert.IsTrue(this.baseRenameTests.RenameObject<GetSingleAddressApiResponse, SubnetObject>());
            Assert.IsTrue(this.baseRenameTests.RenameObject<GetSingleAddressApiResponse, AddressRangeObject>());
            Assert.IsTrue(this.baseRenameTests.RenameObject<GetSingleAddressGroupApiResponse, AddressGroupObject>());
        }

        [TestMethod]
        [ExpectedException(typeof(AttemptToRenameNonExistingObject))]
        public void RenameNonExistingAddressTest()
        {
            this.baseRenameTests.RenameNonExistingTest<AddressObject>();
        }

        [TestMethod]
        [ExpectedException(typeof(AttemptToRenameNonExistingObject))]
        public void RenameNonExistingSubnetTest()
        {
            this.baseRenameTests.RenameNonExistingTest<SubnetObject>();
        }

        [TestMethod]
        [ExpectedException(typeof(AttemptToRenameNonExistingObject))]
        public void RenameNonExistingAddressRangeTest()
        {
            this.baseRenameTests.RenameNonExistingTest<AddressRangeObject>();
        }

        [TestMethod]
        [ExpectedException(typeof(AttemptToRenameNonExistingObject))]
        public void RenameNonExistingAddressGroupTest()
        {
            this.baseRenameTests.RenameNonExistingTest<AddressGroupObject>();
        }
    }
}
