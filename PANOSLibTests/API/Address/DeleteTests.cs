namespace PANOSLibTest
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
            this.baseDeleteTests.DeleteObject<GetSingleAddressApiResponse, AddressObject>();
            this.baseDeleteTests.DeleteObject<GetSingleAddressApiResponse, SubnetObject>();
            this.baseDeleteTests.DeleteObject<GetSingleAddressApiResponse, AddressRangeObject>();
            this.baseDeleteTests.DeleteObject<GetSingleAddressGroupApiResponse, AddressGroupObject>();
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFound))]
        public void DeleteNonExistingAddressTest()
        {
            this.baseDeleteTests.DeleteNonExistingObject<AddressObject>();
            this.baseDeleteTests.DeleteNonExistingObject<SubnetObject>();
            this.baseDeleteTests.DeleteNonExistingObject<AddressRangeObject>();
            this.baseDeleteTests.DeleteNonExistingObject<AddressGroupObject>();
        }
    }
}
