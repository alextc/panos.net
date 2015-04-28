namespace PANOSLibTest
{
    using System;
    using System.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PANOS;

    [TestClass]
    public class GetSubnetTests : BaseConfigTest
    {
        private readonly GetTests baseGetTests = new GetTests();

        // Running tests against the Running config requires calling Commit, which makes tests much slower
        // Don't forget to switch this on once in a while
        private readonly bool testAgainstRunningConfig = Boolean.Parse(ConfigurationManager.AppSettings["TestAgainstRunningConfig"]);
        
        [TestMethod]
        public void GetAllSubnetsTest()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if(config == ConfigTypes.Running && !this.testAgainstRunningConfig) continue;
                baseGetTests.GetAllObjects<GetAllAddressesApiResponse, SubnetObject>(
                    Schema.AddressSchemaName,
                    config,
                    new RandomSubnetObjectFactory());
            }
        }

        [TestMethod]
        public void GetSingleSubnetTest()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if (config == ConfigTypes.Running && !this.testAgainstRunningConfig) continue;
                baseGetTests.GetSingleObject<GetSingleAddressApiResponse, SubnetObject>(
                    Schema.AddressSchemaName, config,
                    new RandomSubnetObjectFactory());   
            }
        }

        [TestMethod]
        public void GetNonExistingSubnetTest()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if (config == ConfigTypes.Running && !this.testAgainstRunningConfig) continue;
                baseGetTests.GetNonExistingObject<GetSingleAddressApiResponse, SubnetObject>(
                    Schema.AddressSchemaName,
                    config,
                    new RandomSubnetObjectFactory());
            }
        }
    }
}