namespace PANOSLibTest
{
    using System;
    using System.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PANOS;

    [TestClass]
    public class GetAddressRangeTests : BaseConfigTest
    {
        private readonly GetTests<AddressRangeObject, GetSingleAddressApiResponse, GetAllAddressesApiResponse> baseGetTests =
            new GetTests<AddressRangeObject, GetSingleAddressApiResponse, GetAllAddressesApiResponse>(new RandomAddressRangeObjectFactory());

        // Running tests against the Running config requires calling Commit, which makes tests much slower
        // Don't forget to switch this on once in a while
        private readonly bool testAgainstRunningConfig = Boolean.Parse(ConfigurationManager.AppSettings["TestAgainstRunningConfig"]);
        
        [TestMethod]
        public void GetAllAddressRangesTest()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if(config == ConfigTypes.Running && !this.testAgainstRunningConfig) continue;
                baseGetTests.GetAllObjects(Schema.AddressSchemaName, config);
            }
        }

        [TestMethod]
        public void GetSingleAddressRangeTest()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if (config == ConfigTypes.Running && !this.testAgainstRunningConfig) continue;
                baseGetTests.GetSingleObject(Schema.AddressSchemaName, config);   
            }
        }

        [TestMethod]
        public void GetNonExistingAddressRangeTest()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if (config == ConfigTypes.Running && !this.testAgainstRunningConfig) continue;
                baseGetTests.GetNonExistingObject(Schema.AddressSchemaName, config);
            }
        }
    }
}