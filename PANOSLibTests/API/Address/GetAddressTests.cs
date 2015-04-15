namespace PANOSLibTest.API.Address
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PANOS;

    [TestClass]
    public class GetAddressTests 
    {
        private readonly GetTests baseGetTests = new GetTests();

        [TestMethod]
        public void GetAllAddressesFromRunningConfigTest()
        {
            Assert.IsTrue(
                baseGetTests.GetAllObjects<GetAllAddressesApiResponse, AddressObject>(Schema.AddressSchemaName, ConfigTypes.Running));
            Assert.IsTrue(
               baseGetTests.GetAllObjects<GetAllAddressesApiResponse, SubnetObject>(Schema.AddressSchemaName, ConfigTypes.Running));
            Assert.IsTrue(
               baseGetTests.GetAllObjects<GetAllAddressesApiResponse, AddressRangeObject>(Schema.AddressSchemaName, ConfigTypes.Running));
            Assert.IsTrue(
                baseGetTests.GetAllObjects<GetAllAddressGroupApiResponse, AddressGroupObject>(Schema.AddressGroupSchemaName, ConfigTypes.Running));
        }
        
        [TestMethod]
        public void GetAllAddressesFromCandidateConfigTest()
        {
            Assert.IsTrue(
                 baseGetTests.GetAllObjects<GetAllAddressesApiResponse, AddressObject>(Schema.AddressSchemaName, ConfigTypes.Candidate));
            Assert.IsTrue(
                 baseGetTests.GetAllObjects<GetAllAddressesApiResponse, SubnetObject>(Schema.AddressSchemaName, ConfigTypes.Candidate));
            Assert.IsTrue(
                 baseGetTests.GetAllObjects<GetAllAddressesApiResponse, AddressRangeObject>(Schema.AddressSchemaName, ConfigTypes.Candidate));
            Assert.IsTrue(
                baseGetTests.GetAllObjects<GetAllAddressGroupApiResponse, AddressGroupObject>(Schema.AddressGroupSchemaName, ConfigTypes.Candidate)); 
        }
        
        [TestMethod]
        public void GetSingleAddressFromRunningConfigTest()
        {
            Assert.IsTrue(
                baseGetTests.GetSingleObject<GetSingleAddressApiResponse, AddressObject>(Schema.AddressSchemaName, ConfigTypes.Running));
            Assert.IsTrue(
                baseGetTests.GetSingleObject<GetSingleAddressApiResponse, SubnetObject>(Schema.AddressSchemaName, ConfigTypes.Running));
            Assert.IsTrue(
                baseGetTests.GetSingleObject<GetSingleAddressApiResponse, AddressRangeObject>(Schema.AddressSchemaName, ConfigTypes.Running));
        }

        [TestMethod]
        public void GetSingleAddressFromCandidateConfigTest()
        {
            Assert.IsTrue(
                baseGetTests.GetSingleObject<GetSingleAddressApiResponse, AddressObject>(Schema.AddressSchemaName, ConfigTypes.Candidate));
            Assert.IsTrue(
                baseGetTests.GetSingleObject<GetSingleAddressApiResponse, SubnetObject>(Schema.AddressSchemaName, ConfigTypes.Candidate));
            Assert.IsTrue(
                baseGetTests.GetSingleObject<GetSingleAddressApiResponse, AddressRangeObject>(Schema.AddressSchemaName, ConfigTypes.Candidate));
            Assert.IsTrue(
                baseGetTests.GetSingleObject<GetSingleAddressGroupApiResponse, AddressGroupObject>(Schema.AddressGroupSchemaName, ConfigTypes.Candidate));
        }

        [TestMethod]
        public void GetNonExistingAddressFromCandidateConfigTest()
        {
            Assert.IsTrue(
                baseGetTests.GetNonExistingObject<GetSingleAddressApiResponse, AddressObject>(Schema.AddressSchemaName, ConfigTypes.Candidate));
            Assert.IsTrue(
                baseGetTests.GetNonExistingObject<GetSingleAddressApiResponse, SubnetObject>(Schema.AddressSchemaName, ConfigTypes.Candidate));
            Assert.IsTrue(
                baseGetTests.GetNonExistingObject<GetSingleAddressApiResponse, AddressRangeObject>(Schema.AddressSchemaName, ConfigTypes.Candidate));
            Assert.IsTrue(
                baseGetTests.GetNonExistingObject<GetSingleAddressGroupApiResponse, AddressGroupObject>(Schema.AddressGroupSchemaName, ConfigTypes.Candidate));
        }

        [TestMethod]
        public void GetNonExistingAddressFromRunningConfigTest()
        {
            Assert.IsTrue(
                baseGetTests.GetNonExistingObject<GetSingleAddressApiResponse, AddressObject>(Schema.AddressSchemaName, ConfigTypes.Running));
            Assert.IsTrue(
                baseGetTests.GetNonExistingObject<GetSingleAddressApiResponse, SubnetObject>(Schema.AddressSchemaName, ConfigTypes.Running));
            Assert.IsTrue(
                baseGetTests.GetNonExistingObject<GetSingleAddressApiResponse, AddressRangeObject>(Schema.AddressSchemaName, ConfigTypes.Running));
            Assert.IsTrue(
                baseGetTests.GetNonExistingObject<GetSingleAddressGroupApiResponse, AddressGroupObject>(Schema.AddressGroupSchemaName, ConfigTypes.Running));
        }
    }
}