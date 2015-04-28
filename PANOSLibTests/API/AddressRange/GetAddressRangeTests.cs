namespace PANOSLibTest
{
    using System;
    using System.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PANOS;

    [TestClass]
    public class GetAddressRangeTests : BaseConfigTest
    {
        private readonly GetTests baseGetTests = new GetTests();

        // Running tests against the Running config requires calling Commit, which makes tests much slower
        // Don't forget to switch this on once in a while
        private readonly bool testAgainstRunningConfig = Boolean.Parse(ConfigurationManager.AppSettings["TestAgainstRunningConfig"]);
        
        [TestMethod]
        public void GetAllAddressRangesTest()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if(config == ConfigTypes.Running && !this.testAgainstRunningConfig) continue;
                baseGetTests.GetAllObjects<GetAllAddressesApiResponse, AddressRangeObject>(
                    Schema.AddressSchemaName,
                    config,
                    new RandomAddressRangeObjectFactory());
            }
        }

        [TestMethod]
        public void GetSingleAddressRangeTest()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if (config == ConfigTypes.Running && !this.testAgainstRunningConfig) continue;
                baseGetTests.GetSingleObject<GetSingleAddressApiResponse, AddressRangeObject>(
                    Schema.AddressSchemaName,
                    config,
                    new RandomAddressRangeObjectFactory());   
            }
        }

        [TestMethod]
        public void GetNonExistingAddressRangeTest()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if (config == ConfigTypes.Running && !this.testAgainstRunningConfig) continue;
                baseGetTests.GetNonExistingObject<GetSingleAddressApiResponse, AddressRangeObject>(
                    Schema.AddressSchemaName,
                    config,
                    new RandomAddressRangeObjectFactory());
            }
        }
    }
}