namespace PANOSPsTest
{
    using NUnit.Framework;
    using PANOS;
    
    [TestFixture]
    public class DeleteTests : BasePsTest
    {
        private readonly PsDeleteTests deleteTests = new PsDeleteTests();

        [Test]
        public void DeleteSingleAddresByObjectPassedAsParameter()
        {
            deleteTests.DeleteSingleByObjectPassedAsParameter<GetSingleAddressApiResponse, AddressObject>("PANOSAddress");
            // deleteTests.DeleteSingleByObjectPassedAsParameter<GetSingleAddressApiResponse, AddressObject>("PANOSSubnet");
            // deleteTests.DeleteSingleByObjectPassedAsParameter<GetSingleAddressApiResponse, AddressObject>("PANOSRange");
            // deleteTests.DeleteSingleByObjectPassedAsParameter<GetSingleAddressApiResponse, AddressObject>("PANOSAddressGroup");
        }

        [Test]
        public void DeleteSingleAddressByNamePassedAsParameter()
        {
            deleteTests.DeleteSingleByNamePassedAsParameter<GetSingleAddressApiResponse, AddressObject>("PANOSAddress");
            //deleteTests.DeleteSingleByNamePassedAsParameter<GetSingleAddressApiResponse, SubnetObject>());
            //deleteTests.DeleteSingleByNamePassedAsParameter<GetSingleAddressApiResponse, AddressRangeObject>());
            //deleteTests.DeleteSingleByNamePassedAsParameter<GetSingleAddressGroupApiResponse, AddressGroupObject>());
        }

        [Test]
        public void DeleteMultipleAddressesFromObjectsPassedAsParameter()
        {
            deleteTests.DeleteMultipleByObjectPassedAsParameter<GetSingleAddressApiResponse, AddressObject>("PANOSAddress");
            //deleteTests.DeleteMultipleByObjectPassedAsParameter<GetSingleAddressApiResponse, SubnetObject>());
            //deleteTests.DeleteMultipleByObjectPassedAsParameter<GetSingleAddressApiResponse, AddressRangeObject>());
            //deleteTests.DeleteMultipleByObjectPassedAsParameter<GetSingleAddressGroupApiResponse, AddressGroupObject>());
        }

        [Test]
        public void DeleteMultipleAddressesByNamesPassedAsParameter()
        {
            deleteTests.DeleteMultipleByNamePassedAsParameter<GetSingleAddressApiResponse, AddressObject>("PANOSAddress");
            //deleteTests.DeleteMultipleByNamePassedAsParameter<GetSingleAddressApiResponse, SubnetObject>());
            //deleteTests.DeleteMultipleByNamePassedAsParameter<GetSingleAddressApiResponse, AddressRangeObject>());
            //deleteTests.DeleteMultipleByNamePassedAsParameter<GetSingleAddressGroupApiResponse, AddressGroupObject>());
        }

        [Test]
        public void DeleteMultipleAddressesByNamesPassedViaPipeline()
        {
            deleteTests.DeleteMultipleByNamePassedViaPipeline<GetSingleAddressApiResponse, AddressObject>("PANOSAddress");
            //deleteTests.DeleteMultipleByNamePassedViaPipeline<GetSingleAddressApiResponse, SubnetObject>());
            //deleteTests.DeleteMultipleByNamePassedViaPipeline<GetSingleAddressApiResponse, AddressRangeObject>());
            //deleteTests.DeleteMultipleByNamePassedViaPipeline<GetSingleAddressGroupApiResponse, AddressGroupObject>());
        }

        [Test]
        public void DeleteMultipleAddressesByObjectPassedViaPipeline()
        {
            deleteTests.DeleteMultipleByObjectPassedViaPipeline<GetSingleAddressApiResponse, AddressObject>("PANOSAddress");
            //deleteTests.DeleteMultipleByObjectPassedViaPipeline<GetSingleAddressApiResponse, SubnetObject>());
            //deleteTests.DeleteMultipleByObjectPassedViaPipeline<GetSingleAddressApiResponse, AddressRangeObject>());
            //deleteTests.DeleteMultipleByObjectPassedViaPipeline<GetSingleAddressGroupApiResponse, AddressGroupObject>());
        }

        [Test]
        public void DeleteAndPassThruObject()
        {
            deleteTests.DeleteAndPassThruObjectTest<GetSingleAddressApiResponse, AddressObject>("PANOSAddress");
        }

        [Test]
        public void DeleteAndPassThruName()
        {
            deleteTests.DeleteAndPassThruName<GetSingleAddressApiResponse, AddressObject>("PANOSAddress");
        }
    }    
}
 
