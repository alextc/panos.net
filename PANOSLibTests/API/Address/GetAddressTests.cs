namespace PANOSLibTest
{
    using System;
    using System.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PANOS;

    [TestClass]
    public class GetAddressTests : BaseConfigTest
    {
        private readonly GetTests<AddressObject, GetSingleAddressApiResponse, GetAllAddressesApiResponse> baseGetTests = 
            new GetTests<AddressObject, GetSingleAddressApiResponse, GetAllAddressesApiResponse>(new RandomAddressObjectFactory());

        // Running tests against the Running config requires calling Commit, which makes tests much slower
        // Don't forget to switch this on once in a while
        private readonly bool testAgainstRunningConfig = Boolean.Parse(ConfigurationManager.AppSettings["TestAgainstRunningConfig"]);
        
        [TestMethod]
        public void GetAllAddressesTest()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if(config == ConfigTypes.Running && !this.testAgainstRunningConfig) continue;
                baseGetTests.GetAllObjects(Schema.AddressSchemaName, config);
            }
        }

        [TestMethod]
        public void GetSingleAddressTest()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if (config == ConfigTypes.Running && !this.testAgainstRunningConfig) continue;
                baseGetTests.GetSingleObject(Schema.AddressSchemaName, config);   
            }
        }

        [TestMethod]
        public void GetNonExistingAddressTest()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if (config == ConfigTypes.Running && !this.testAgainstRunningConfig) continue;
                baseGetTests.GetNonExistingObject(Schema.AddressSchemaName, config);
            }
        }
    }
}