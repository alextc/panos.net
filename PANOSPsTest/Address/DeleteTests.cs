namespace PANOSPsTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using PANOS;

    using PANOSLibTest;

    [TestClass]
    public class DeleteTests : BaseConfigTest
    {
        private readonly PsDeleteTests deleteTests = new PsDeleteTests();

        [TestMethod]
        public void DeleteSingleAddresByObjectPassedAsParameter()
        {
            Assert.IsTrue(this.deleteTests.DeleteSingleByObjectPassedAsParameter<GetSingleAddressApiResponse, AddressObject>());
            Assert.IsTrue(this.deleteTests.DeleteSingleByObjectPassedAsParameter<GetSingleAddressApiResponse, SubnetObject>());
            Assert.IsTrue(this.deleteTests.DeleteSingleByObjectPassedAsParameter<GetSingleAddressApiResponse, AddressRangeObject>());
            Assert.IsTrue(this.deleteTests.DeleteSingleByObjectPassedAsParameter<GetSingleAddressGroupApiResponse, AddressGroupObject>());
        }

        [TestMethod]
        public void DeleteSingleAddressByNamePassedAsParameter()
        {
            Assert.IsTrue(this.deleteTests.DeleteSingleByNamePassedAsParameter<GetSingleAddressApiResponse, AddressObject>());
            Assert.IsTrue(this.deleteTests.DeleteSingleByNamePassedAsParameter<GetSingleAddressApiResponse, SubnetObject>());
            Assert.IsTrue(this.deleteTests.DeleteSingleByNamePassedAsParameter<GetSingleAddressApiResponse, AddressRangeObject>());
            Assert.IsTrue(this.deleteTests.DeleteSingleByNamePassedAsParameter<GetSingleAddressGroupApiResponse, AddressGroupObject>());
        }

        [TestMethod]
        public void DeleteMultipleAddressesFromObjectsPassedAsParameter()
        {
            Assert.IsTrue(this.deleteTests.DeleteMultipleByObjectPassedAsParameter<GetSingleAddressApiResponse, AddressObject>());
            Assert.IsTrue(this.deleteTests.DeleteMultipleByObjectPassedAsParameter<GetSingleAddressApiResponse, SubnetObject>());
            Assert.IsTrue(this.deleteTests.DeleteMultipleByObjectPassedAsParameter<GetSingleAddressApiResponse, AddressRangeObject>());
            Assert.IsTrue(this.deleteTests.DeleteMultipleByObjectPassedAsParameter<GetSingleAddressGroupApiResponse, AddressGroupObject>());
        }

        [TestMethod]
        public void DeleteMultipleAddressesByNamesPassedAsParameter()
        {
            Assert.IsTrue(this.deleteTests.DeleteMultipleByNamePassedAsParameter<GetSingleAddressApiResponse, AddressObject>());
            Assert.IsTrue(this.deleteTests.DeleteMultipleByNamePassedAsParameter<GetSingleAddressApiResponse, SubnetObject>());
            Assert.IsTrue(this.deleteTests.DeleteMultipleByNamePassedAsParameter<GetSingleAddressApiResponse, AddressRangeObject>());
            Assert.IsTrue(this.deleteTests.DeleteMultipleByNamePassedAsParameter<GetSingleAddressGroupApiResponse, AddressGroupObject>());
        }

        [TestMethod]
        public void DeleteMultipleAddressesByNamesPassedViaPipeline()
        {
            Assert.IsTrue(this.deleteTests.DeleteMultipleByNamePassedViaPipeline<GetSingleAddressApiResponse, AddressObject>());
            Assert.IsTrue(this.deleteTests.DeleteMultipleByNamePassedViaPipeline<GetSingleAddressApiResponse, SubnetObject>());
            Assert.IsTrue(this.deleteTests.DeleteMultipleByNamePassedViaPipeline<GetSingleAddressApiResponse, AddressRangeObject>());
            Assert.IsTrue(this.deleteTests.DeleteMultipleByNamePassedViaPipeline<GetSingleAddressGroupApiResponse, AddressGroupObject>());
        }

        [TestMethod]
        public void DeleteMultipleAddressesByObjectPassedViaPipeline()
        {
            Assert.IsTrue(this.deleteTests.DeleteMultipleByObjectPassedViaPipeline<GetSingleAddressApiResponse, AddressObject>());
            Assert.IsTrue(this.deleteTests.DeleteMultipleByObjectPassedViaPipeline<GetSingleAddressApiResponse, SubnetObject>());
            Assert.IsTrue(this.deleteTests.DeleteMultipleByObjectPassedViaPipeline<GetSingleAddressApiResponse, AddressRangeObject>());
            Assert.IsTrue(this.deleteTests.DeleteMultipleByObjectPassedViaPipeline<GetSingleAddressGroupApiResponse, AddressGroupObject>());
        }
    }    
}
 
