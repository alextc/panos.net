namespace PANOSLibTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PANOS;

    [TestClass]
    public class GetTestsRunner : BaseConfigTest
    {
        private readonly GetTests<AddressObject, GetSingleAddressApiResponse, GetAllAddressesApiResponse> addressGetTests;
        private readonly GetTests<SubnetObject, GetSingleAddressApiResponse, GetAllAddressesApiResponse> subnetGetTests;
        private readonly GetTests<AddressRangeObject, GetSingleAddressApiResponse, GetAllAddressesApiResponse> addressRangeGetTests;
        private readonly GetTests<AddressGroupObject, GetSingleAddressGroupApiResponse, GetAllAddressGroupApiResponse> addressGroupGetTests;
            
        public GetTestsRunner()
        {
            addressGetTests = new GetTests<AddressObject, GetSingleAddressApiResponse, GetAllAddressesApiResponse>(
                 new RandomAddressObjectFactory(),
                 Schema.AddressSchemaName,
                 ConfigTypes.Candidate);

            subnetGetTests = new GetTests<SubnetObject, GetSingleAddressApiResponse, GetAllAddressesApiResponse>(
                new RandomSubnetObjectFactory(),
                Schema.AddressSchemaName,
                ConfigTypes.Candidate);

            addressRangeGetTests = 
                new GetTests<AddressRangeObject, GetSingleAddressApiResponse, GetAllAddressesApiResponse>(
                    new RandomAddressRangeObjectFactory(),
                    Schema.AddressSchemaName,
                    ConfigTypes.Candidate);

            addressGroupGetTests  = new GetTests<AddressGroupObject, GetSingleAddressGroupApiResponse, GetAllAddressGroupApiResponse>(
                new RandomAddressGroupObjectFactory(
                    new AddableRepository(ConfigCommandFactory)),
                    Schema.AddressGroupSchemaName,
                    ConfigTypes.Candidate);
        }
        
        [TestMethod]
        public void ShouldGetAllObjects()
        {
            addressGetTests.ShouldGetAllObjects();
            subnetGetTests.ShouldGetAllObjects();
            addressRangeGetTests.ShouldGetAllObjects();
            addressGroupGetTests.ShouldGetAllObjects();
        }

        [TestMethod]
        public void ShouldGetSingleObjectRequestedByName()
        {
            addressGetTests.ShouldGetSingleObjectRequestedByName();
            subnetGetTests.ShouldGetSingleObjectRequestedByName();
            addressRangeGetTests.ShouldGetSingleObjectRequestedByName();
            addressGroupGetTests.ShouldGetSingleObjectRequestedByName();
        }

        [TestMethod]
        public void ShouldNotGetAnythingWhenNonExistingNameSupplied()
        {
            addressGetTests.ShouldNotGetAnythingWhenNonExistingNameSupplied();
            subnetGetTests.ShouldNotGetAnythingWhenNonExistingNameSupplied();
            addressRangeGetTests.ShouldNotGetAnythingWhenNonExistingNameSupplied();
            addressGroupGetTests.ShouldNotGetAnythingWhenNonExistingNameSupplied();
        }
    }
}
