namespace PANOSLibTest
{
    using System;
    using System.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PANOS;

    [TestClass]
    public class GetAddressTests : BaseConfigTest
    {
        private readonly GetTests baseGetTests = new GetTests();

        // Running tests against the Running config requires calling Commit, which makes tests much slower
        // Don't forget to switch this on once in a while
        private readonly bool testAgainstRunningConfig = Boolean.Parse(ConfigurationManager.AppSettings["TestAgainstRunningConfig"]);
        
        [TestMethod]
        public void GetAllAddressesTest()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if(config == ConfigTypes.Running && !this.testAgainstRunningConfig) continue;
                baseGetTests.GetAllObjects<GetAllAddressesApiResponse, AddressObject>(Schema.AddressSchemaName, config, new RandomAddressObjectFactory());
            }
        }

        [TestMethod]
        public void GetSingleAddressTest()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if (config == ConfigTypes.Running && !this.testAgainstRunningConfig) continue;
                baseGetTests.GetSingleObject<GetSingleAddressApiResponse, AddressObject>(Schema.AddressSchemaName, config, new RandomAddressObjectFactory());   
            }
        }

        [TestMethod]
        public void GetNonExistingAddressTest()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if (config == ConfigTypes.Running && !this.testAgainstRunningConfig) continue;
                baseGetTests.GetNonExistingObject<GetSingleAddressApiResponse, AddressObject>(Schema.AddressSchemaName, config, new RandomAddressObjectFactory());
            }
        }
    }
}