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
                 Schema.AddressSchemaName);

            subnetGetTests = new GetTests<SubnetObject, GetSingleAddressApiResponse, GetAllAddressesApiResponse>(
                new RandomSubnetObjectFactory(),
                Schema.AddressSchemaName);

            addressRangeGetTests = 
                new GetTests<AddressRangeObject, GetSingleAddressApiResponse, GetAllAddressesApiResponse>(
                    new RandomAddressRangeObjectFactory(),
                    Schema.AddressSchemaName);

            addressGroupGetTests  = new GetTests<AddressGroupObject, GetSingleAddressGroupApiResponse, GetAllAddressGroupApiResponse>(
                new RandomAddressGroupObjectFactory(
                    new AddableRepository(ConfigCommandFactory)),
                    Schema.AddressGroupSchemaName);
        }


        [TestMethod]
        public void GetAllTests()
        {
            addressGetTests.GetAllObjects(ConfigTypes.Candidate);
            subnetGetTests.GetAllObjects(ConfigTypes.Candidate);
            addressRangeGetTests.GetAllObjects(ConfigTypes.Candidate);
            addressGroupGetTests.GetAllObjects(ConfigTypes.Candidate);
        }

        [TestMethod]
        public void GetSingleTests()
        {
            addressGetTests.GetSingleObject(ConfigTypes.Candidate);
            subnetGetTests.GetSingleObject(ConfigTypes.Candidate);
            addressRangeGetTests.GetSingleObject(ConfigTypes.Candidate);
            addressGroupGetTests.GetAllObjects(ConfigTypes.Candidate);
        }

        [TestMethod]
        public void GetNonExistingTests()
        {
            addressGetTests.GetNonExistingObject(ConfigTypes.Candidate);
            subnetGetTests.GetNonExistingObject(ConfigTypes.Candidate);
            addressRangeGetTests.GetNonExistingObject(ConfigTypes.Candidate);
            addressGroupGetTests.GetNonExistingObject(ConfigTypes.Candidate);
        }
    }
}
