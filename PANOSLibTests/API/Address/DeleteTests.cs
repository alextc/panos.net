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
            baseDeleteTests.DeleteObject<GetSingleAddressApiResponse, AddressObject>();
            baseDeleteTests.DeleteObject<GetSingleAddressApiResponse, SubnetObject>();
            baseDeleteTests.DeleteObject<GetSingleAddressApiResponse, AddressRangeObject>();
            baseDeleteTests.DeleteObject<GetSingleAddressGroupApiResponse, AddressGroupObject>();
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFound))]
        public void DeleteNonExistingAddressTest()
        {
            baseDeleteTests.DeleteNonExistingObject<AddressObject>();
            baseDeleteTests.DeleteNonExistingObject<SubnetObject>();
            baseDeleteTests.DeleteNonExistingObject<AddressRangeObject>();
            baseDeleteTests.DeleteNonExistingObject<AddressGroupObject>();
        }
    }
}
