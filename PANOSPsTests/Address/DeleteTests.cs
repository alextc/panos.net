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
            deleteTests.DeleteSingleByObjectPassedAsParameter<GetSingleAddressApiResponse, AddressObject>("PANOSAddress");
            // deleteTests.DeleteSingleByObjectPassedAsParameter<GetSingleAddressApiResponse, AddressObject>("PANOSSubnet");
            // deleteTests.DeleteSingleByObjectPassedAsParameter<GetSingleAddressApiResponse, AddressObject>("PANOSRange");
            // deleteTests.DeleteSingleByObjectPassedAsParameter<GetSingleAddressApiResponse, AddressObject>("PANOSAddressGroup");
        }

        [TestMethod]
        public void DeleteSingleAddressByNamePassedAsParameter()
        {
            deleteTests.DeleteSingleByNamePassedAsParameter<GetSingleAddressApiResponse, AddressObject>("PANOSAddress");
            //deleteTests.DeleteSingleByNamePassedAsParameter<GetSingleAddressApiResponse, SubnetObject>());
            //deleteTests.DeleteSingleByNamePassedAsParameter<GetSingleAddressApiResponse, AddressRangeObject>());
            //deleteTests.DeleteSingleByNamePassedAsParameter<GetSingleAddressGroupApiResponse, AddressGroupObject>());
        }

        [TestMethod]
        public void DeleteMultipleAddressesFromObjectsPassedAsParameter()
        {
            deleteTests.DeleteMultipleByObjectPassedAsParameter<GetSingleAddressApiResponse, AddressObject>("PANOSAddress");
            //deleteTests.DeleteMultipleByObjectPassedAsParameter<GetSingleAddressApiResponse, SubnetObject>());
            //deleteTests.DeleteMultipleByObjectPassedAsParameter<GetSingleAddressApiResponse, AddressRangeObject>());
            //deleteTests.DeleteMultipleByObjectPassedAsParameter<GetSingleAddressGroupApiResponse, AddressGroupObject>());
        }

        [TestMethod]
        public void DeleteMultipleAddressesByNamesPassedAsParameter()
        {
            deleteTests.DeleteMultipleByNamePassedAsParameter<GetSingleAddressApiResponse, AddressObject>("PANOSAddress");
            //deleteTests.DeleteMultipleByNamePassedAsParameter<GetSingleAddressApiResponse, SubnetObject>());
            //deleteTests.DeleteMultipleByNamePassedAsParameter<GetSingleAddressApiResponse, AddressRangeObject>());
            //deleteTests.DeleteMultipleByNamePassedAsParameter<GetSingleAddressGroupApiResponse, AddressGroupObject>());
        }

        [TestMethod]
        public void DeleteMultipleAddressesByNamesPassedViaPipeline()
        {
            deleteTests.DeleteMultipleByNamePassedViaPipeline<GetSingleAddressApiResponse, AddressObject>("PANOSAddress");
            //deleteTests.DeleteMultipleByNamePassedViaPipeline<GetSingleAddressApiResponse, SubnetObject>());
            //deleteTests.DeleteMultipleByNamePassedViaPipeline<GetSingleAddressApiResponse, AddressRangeObject>());
            //deleteTests.DeleteMultipleByNamePassedViaPipeline<GetSingleAddressGroupApiResponse, AddressGroupObject>());
        }

        [TestMethod]
        public void DeleteMultipleAddressesByObjectPassedViaPipeline()
        {
            deleteTests.DeleteMultipleByObjectPassedViaPipeline<GetSingleAddressApiResponse, AddressObject>("PANOSAddress");
            //deleteTests.DeleteMultipleByObjectPassedViaPipeline<GetSingleAddressApiResponse, SubnetObject>());
            //deleteTests.DeleteMultipleByObjectPassedViaPipeline<GetSingleAddressApiResponse, AddressRangeObject>());
            //deleteTests.DeleteMultipleByObjectPassedViaPipeline<GetSingleAddressGroupApiResponse, AddressGroupObject>());
        }

        [TestMethod]
        public void DeleteAndPassThruObject()
        {
            deleteTests.DeleteAndPassThruObjectTest<GetSingleAddressApiResponse, AddressObject>("PANOSAddress");
        }

        [TestMethod]
        public void DeleteAndPassThruName()
        {
            deleteTests.DeleteAndPassThruName<GetSingleAddressApiResponse, AddressObject>("PANOSAddress");
        }
    }    
}
 
