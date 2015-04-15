namespace PANOSLibTest.API.Address
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using PANOS;

    [TestClass]
    public class DeleteAddressTests 
    {
        readonly DeleteTests baseDeleteTests = new DeleteTests();

        [TestMethod]
        public void DeleteTest()
        {
            Assert.IsTrue(baseDeleteTests.DeleteObject<GetSingleAddressApiResponse, AddressObject>());
            Assert.IsTrue(baseDeleteTests.DeleteObject<GetSingleAddressApiResponse, SubnetObject>());
            Assert.IsTrue(baseDeleteTests.DeleteObject<GetSingleAddressApiResponse, AddressRangeObject>());
            Assert.IsTrue(baseDeleteTests.DeleteObject<GetSingleAddressGroupApiResponse, AddressGroupObject>());
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFound))]
        public void DeleteNonExistingAddressTest()
        {
            baseDeleteTests.DeleteNonExistingObject<AddressObject>();
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFound))]
        public void DeleteNonExistingSubnetTest()
        {
            baseDeleteTests.DeleteNonExistingObject<SubnetObject>();
        }
        [TestMethod]
        [ExpectedException(typeof(ObjectNotFound))]
        public void DeleteNonExistingAddressRangeTest()
        {
            baseDeleteTests.DeleteNonExistingObject<AddressRangeObject>();
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFound))]
        public void DeleteNonExistingAddressGroupTest()
        {
            baseDeleteTests.DeleteNonExistingObject<AddressGroupObject>();
        }
    }
}
