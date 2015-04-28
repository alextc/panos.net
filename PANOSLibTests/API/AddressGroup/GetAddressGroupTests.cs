namespace PANOSLibTest
{
    using System;
    using System.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PANOS;

    [TestClass]
    public class GetAddressGroupTests : BaseConfigTest
    {
        private readonly GetTests baseGetTests = new GetTests();
        private readonly IAddableRepository addableRepository;

        // Running tests against the Running config requires calling Commit, which makes tests much slower
        // Don't forget to switch this on once in a while
        private readonly bool testAgainstRunningConfig = Boolean.Parse(ConfigurationManager.AppSettings["TestAgainstRunningConfig"]);

        public GetAddressGroupTests()
        {
            addableRepository = new AddableRepository(ConfigCommandFactory);
        }
        
        [TestMethod]
        public void GetAllAddressesGroupTest()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if(config == ConfigTypes.Running && !this.testAgainstRunningConfig) continue;
                baseGetTests.GetAllObjects<GetAllAddressGroupApiResponse, AddressGroupObject>(
                    Schema.AddressGroupSchemaName,
                    config,
                    new RandomAddressGroupObjectFactory(addableRepository));
            }
        }

        [TestMethod]
        public void GetSingleAddressGroupTest()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if (config == ConfigTypes.Running && !this.testAgainstRunningConfig) continue;
                baseGetTests.GetSingleObject<GetSingleAddressGroupApiResponse, AddressGroupObject>(
                    Schema.AddressGroupSchemaName,
                    config,
                    new RandomAddressGroupObjectFactory(addableRepository));   
            }
        }

        [TestMethod]
        public void GetNonExistingAddressGroupTest()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if (config == ConfigTypes.Running && !this.testAgainstRunningConfig) continue;
                baseGetTests.GetNonExistingObject<GetSingleAddressGroupApiResponse, AddressGroupObject>(
                    Schema.AddressGroupSchemaName,
                    config,
                    new RandomAddressGroupObjectFactory(addableRepository));
            }
        }
    }
}