namespace PANOSLibTest.API.Address
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
                if(config == ConfigTypes.Running && !testAgainstRunningConfig) continue;
                Assert.IsTrue(baseGetTests.GetAllObjects<GetAllAddressesApiResponse, AddressObject>(Schema.AddressSchemaName, config));
                Assert.IsTrue(baseGetTests.GetAllObjects<GetAllAddressesApiResponse, SubnetObject>(Schema.AddressSchemaName, config));
                Assert.IsTrue(baseGetTests.GetAllObjects<GetAllAddressesApiResponse, AddressRangeObject>(Schema.AddressSchemaName, config));
                Assert.IsTrue(baseGetTests.GetAllObjects<GetAllAddressGroupApiResponse, AddressGroupObject>(Schema.AddressGroupSchemaName, config));
            }
        }

        [TestMethod]
        public void GetSingleAddressTest()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if (config == ConfigTypes.Running && !testAgainstRunningConfig) continue;
                Assert.IsTrue(baseGetTests.GetSingleObject<GetSingleAddressApiResponse, AddressObject>(Schema.AddressSchemaName, config));
                Assert.IsTrue(baseGetTests.GetSingleObject<GetSingleAddressApiResponse, SubnetObject>(Schema.AddressSchemaName, config));
                Assert.IsTrue(baseGetTests.GetSingleObject<GetSingleAddressApiResponse, AddressRangeObject>(Schema.AddressSchemaName, config));
            }
        }

        [TestMethod]
        public void GetNonExistingAddressTest()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if (config == ConfigTypes.Running && !testAgainstRunningConfig) continue;
                Assert.IsTrue(baseGetTests.GetNonExistingObject<GetSingleAddressApiResponse, AddressObject>(Schema.AddressSchemaName, config));
                Assert.IsTrue(baseGetTests.GetNonExistingObject<GetSingleAddressApiResponse, SubnetObject>(Schema.AddressSchemaName, config));
                Assert.IsTrue(baseGetTests.GetNonExistingObject<GetSingleAddressApiResponse, AddressRangeObject>(Schema.AddressSchemaName, config));
                Assert.IsTrue(baseGetTests.GetNonExistingObject<GetSingleAddressGroupApiResponse, AddressGroupObject>(Schema.AddressGroupSchemaName, config));
            }
        }
    }
}