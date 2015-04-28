namespace PANOSLibTest
{
    using System;
    using System.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PANOS;

    [TestClass]
    public class GetAddressGroupTests : BaseConfigTest
    {
        private readonly GetTests<AddressGroupObject, GetSingleAddressGroupApiResponse, GetAllAddressGroupApiResponse> baseGetTests;
        
        // Running tests against the Running config requires calling Commit, which makes tests much slower
        // Don't forget to switch this on once in a while
        private readonly bool testAgainstRunningConfig = Boolean.Parse(ConfigurationManager.AppSettings["TestAgainstRunningConfig"]);

        public GetAddressGroupTests()
        {
            baseGetTests = new GetTests<AddressGroupObject, GetSingleAddressGroupApiResponse, GetAllAddressGroupApiResponse>(
                new RandomAddressGroupObjectFactory(new AddableRepository(ConfigCommandFactory)));
        }
        
        [TestMethod]
        public void GetAllAddressesGroupTest()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if(config == ConfigTypes.Running && !this.testAgainstRunningConfig) continue;
                baseGetTests.GetAllObjects(Schema.AddressGroupSchemaName, config);
            }
        }

        [TestMethod]
        public void GetSingleAddressGroupTest()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if (config == ConfigTypes.Running && !this.testAgainstRunningConfig) continue;
                baseGetTests.GetSingleObject(Schema.AddressGroupSchemaName, config);   
            }
        }

        [TestMethod]
        public void GetNonExistingAddressGroupTest()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if (config == ConfigTypes.Running && !this.testAgainstRunningConfig) continue;
                baseGetTests.GetNonExistingObject(Schema.AddressGroupSchemaName, config);
            }
        }
    }
}