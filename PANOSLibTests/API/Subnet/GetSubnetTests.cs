namespace PANOSLibTest
{
    using System;
    using System.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PANOS;

    [TestClass]
    public class GetSubnetTests : BaseConfigTest
    {
        private readonly GetTests<SubnetObject, GetSingleAddressApiResponse, GetAllAddressesApiResponse> baseGetTests =
            new GetTests<SubnetObject, GetSingleAddressApiResponse, GetAllAddressesApiResponse>(new RandomSubnetObjectFactory());

        // Running tests against the Running config requires calling Commit, which makes tests much slower
        // Don't forget to switch this on once in a while
        private readonly bool testAgainstRunningConfig = Boolean.Parse(ConfigurationManager.AppSettings["TestAgainstRunningConfig"]);
        
        [TestMethod]
        public void GetAllSubnetsTest()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if(config == ConfigTypes.Running && !this.testAgainstRunningConfig) continue;
                baseGetTests.GetAllObjects(Schema.AddressSchemaName, config);
            }
        }

        [TestMethod]
        public void GetSingleSubnetTest()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if (config == ConfigTypes.Running && !this.testAgainstRunningConfig) continue;
                baseGetTests.GetSingleObject(Schema.AddressSchemaName, config);   
            }
        }

        [TestMethod]
        public void GetNonExistingSubnetTest()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if (config == ConfigTypes.Running && !this.testAgainstRunningConfig) continue;
                baseGetTests.GetNonExistingObject(Schema.AddressSchemaName, config);
            }
        }
    }
}