namespace PANOSPsTest
{
    using System;
    using System.Configuration;
    using System.Management.Automation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PANOS;
    using PANOSLibTest;

    [TestClass]
    public class GetTests : BaseConfigTest
    {
        private readonly PsGetTests psGetTests = new PsGetTests();

        // Running tests against the Running config requires calling Commit, which makes tests much slower
        // Don't forget to switch this on once in a while
        private readonly bool testAgainstRunningConfig = Boolean.Parse(ConfigurationManager.AppSettings["TestAgainstRunningConfig"]);

        [TestMethod]
        public void GetAllAddresses()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if (config == ConfigTypes.Running && !testAgainstRunningConfig) continue;
                Assert.IsTrue(psGetTests.GetAll<AddressObject>("PANOSAddress", config));
                //Assert.IsTrue(psGetTests.GetAll<SubnetObject>("PANOSSubnet", config));
                //Assert.IsTrue(psGetTests.GetAll<AddressRangeObject>("PANOSAddressRange", config));
                //Assert.IsTrue(psGetTests.GetAll<AddressGroupObject>("PANOSAddressGroup", config));
            }
        }

        [TestMethod]
        public void GetSingleAddressByName()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if (config == ConfigTypes.Running && !testAgainstRunningConfig) continue;
                Assert.IsTrue(psGetTests.GetSingleByName<AddressObject>("PANOSAddress", config));
                //Assert.IsTrue(psGetTests.GetSingleByName<SubnetObject>("PANOSSubnet", config));
                //Assert.IsTrue(psGetTests.GetSingleByName<AddressRangeObject>("PANOSAddressRange", config));
                //Assert.IsTrue(psGetTests.GetSingleByName<AddressGroupObject>("PANOSAddressGroup", config));
            }
        }
        
        [TestMethod]
        public void GetSingleAddressByObject()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if (config == ConfigTypes.Running && !testAgainstRunningConfig) continue;
                Assert.IsTrue(psGetTests.GetSingleByObject<AddressObject>("PANOSAddress", config));
                //Assert.IsTrue(psGetTests.GetSingleByObject<SubnetObject>("PANOSSubnet", config));
                //Assert.IsTrue(psGetTests.GetSingleByObject<AddressRangeObject>("PANOSAddressRange", config));
                //Assert.IsTrue(psGetTests.GetSingleByObject<AddressGroupObject>("PANOSAddressGroup", config));
            }
        }

        [TestMethod]
        public void GetSingleAddressByObjectWhereObjectsAreNotEqualButMatchOnName()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if (config == ConfigTypes.Running && !testAgainstRunningConfig) continue;
                Assert.IsTrue(psGetTests.GetSingleByObjectWhereObjectsAreNotEqualButMatchOnName<AddressObject>("PANOSAddress", config));
                //Assert.IsTrue(psGetTests.GetSingleByObjectWhereObjectsAreNotEqualButMatchOnName<SubnetObject>("PANOSSubnet", config));
                //Assert.IsTrue(psGetTests.GetSingleByObjectWhereObjectsAreNotEqualButMatchOnName<AddressRangeObject>("PANOSAddressRange", config));
                //Assert.IsTrue(psGetTests.GetSingleByObjectWhereObjectsAreNotEqualButMatchOnName<AddressGroupObject>("PANOSAddressGroup", config));
            }
        }
        
        [TestMethod]
        public void GetMultipleAddressesByName()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if (config == ConfigTypes.Running && !testAgainstRunningConfig) continue;
                Assert.IsTrue(psGetTests.GetMultipleByName<AddressObject>("PANOSAddress", config));
                //Assert.IsTrue(psGetTests.GetMultipleByName<SubnetObject>("PANOSSubnet", config));
                //Assert.IsTrue(psGetTests.GetMultipleByName<AddressRangeObject>("PANOSAddressRange", config));
                //Assert.IsTrue(psGetTests.GetMultipleByName<AddressGroupObject>("PANOSAddressGroup", config));
            }
        }

        [TestMethod]
        public void GetMultipleAddressesByNameFromPipeline()
        {
            foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
            {
                if (config == ConfigTypes.Running && !testAgainstRunningConfig) continue;
                Assert.IsTrue(psGetTests.GetMultipleByNameFromPipeline<AddressObject>("PANOSAddress", config));
                //Assert.IsTrue(psGetTests.GetMultipleByNameFromPipeline<SubnetObject>("PANOSSubnet", config));
                //Assert.IsTrue(psGetTests.GetMultipleByNameFromPipeline<AddressRangeObject>("PANOSAddressRange", config));
                //Assert.IsTrue(psGetTests.GetMultipleByNameFromPipeline<AddressGroupObject>("PANOSAddressGroup", config));
            }
        }
        
       [TestMethod]
       public void GetMultipleAddressesByNameWhereSomeDoNotExist()
       {
           foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
           {
               if (config == ConfigTypes.Running && !testAgainstRunningConfig) continue;
               Assert.IsTrue(psGetTests.GetMultipleByNameWhereSomeDontExist<AddressObject>("PANOSAddress", config));
               //Assert.IsTrue(psGetTests.GetMultipleByNameWhereSomeDontExist<SubnetObject>("PANOSSubnet", config));
               //Assert.IsTrue(psGetTests.GetMultipleByNameWhereSomeDontExist<AddressRangeObject>("PANOSAddressRange", config));
               //Assert.IsTrue(psGetTests.GetMultipleByNameWhereSomeDontExist<AddressGroupObject>("PANOSAddressGroup", config));
           }
       }

       [TestMethod]
       public void GetMultipleByObjectWhereSomeFailEqual()
       {
           foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
           {
               if (config == ConfigTypes.Running && !testAgainstRunningConfig) continue;
               Assert.IsTrue(psGetTests.GetMultipleByObjectWhereSomeFailEqualsTests<AddressObject>("PANOSAddress", config));
               //Assert.IsTrue(psGetTests.GetMultipleByObjectWhereSomeFailEqualsTests<SubnetObject>("PANOSSubnet", config));
               //Assert.IsTrue(psGetTests.GetMultipleByObjectWhereSomeFailEqualsTests<AddressRangeObject>("PANOSAddressRange", config));
               //Assert.IsTrue(psGetTests.GetMultipleByObjectWhereSomeFailEqualsTests<AddressGroupObject>("PANOSAddressGroup", config));
           }
       }

       [TestMethod]
       public void GetMultipleAddressesByObjectWhereOneFailsEqual()
       {
           foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
           {
               if (config == ConfigTypes.Running && !testAgainstRunningConfig) continue;
               Assert.IsTrue(this.psGetTests.GetMultipleByObject<AddressObject>("PANOSAddress", config));
               //Assert.IsTrue(this.psGetTests.GetMultipleByObject<SubnetObject>("PANOSSubnet", config));
               //Assert.IsTrue(this.psGetTests.GetMultipleByObject<AddressRangeObject>("PANOSAddressRange", config));
               //Assert.IsTrue(this.psGetTests.GetMultipleByObject<AddressGroupObject>("PANOSAddressGroup", config));
           }
       }

       [TestMethod]
       public void GetMultipleAddressesByObjectViaPipeLine()
       {
           foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
           {
               if (config == ConfigTypes.Running && !testAgainstRunningConfig) continue;
               Assert.IsTrue(psGetTests.GetMultipleByObjectFromPipeline<AddressObject>("PANOSAddress", config));
               //Assert.IsTrue(psGetTests.GetMultipleByObjectFromPipeline<SubnetObject>("PANOSSubnet", config));
               //Assert.IsTrue(psGetTests.GetMultipleByObjectFromPipeline<AddressRangeObject>("PANOSAddressRange", config));
               //Assert.IsTrue(psGetTests.GetMultipleByObjectFromPipeline<AddressGroupObject>("PANOSAddressGroup", config));
           }
       }

       [TestMethod]
       public void GetMultipleAddressesByNameViaPipeLine()
       {
           foreach (ConfigTypes config in Enum.GetValues(typeof(ConfigTypes)))
           {
               if (config == ConfigTypes.Running && !testAgainstRunningConfig) continue;
               Assert.IsTrue(psGetTests.GetMultipleByObjectFromPipeline<AddressObject>("PANOSAddress", config));
               //Assert.IsTrue(psGetTests.GetMultipleByObjectFromPipeline<SubnetObject>("PANOSSubnet", config));
               //Assert.IsTrue(psGetTests.GetMultipleByObjectFromPipeline<AddressRangeObject>("PANOSAddressRange", config));
               //Assert.IsTrue(psGetTests.GetMultipleByObjectFromPipeline<AddressGroupObject>("PANOSAddressGroup", config));
           }
       }

        [TestMethod]
        [ExpectedException(typeof(ParameterBindingException), AllowDerivedTypes = true)]
        public void RejectInvalidNameParameter()
        {
            psGetTests.RejectInvalidName("PANOSAddress");
            //psGetTests.RejectInvalidName("PANOSSubnet");
            //psGetTests.RejectInvalidName("PANOSAddressRange");
            //psGetTests.RejectInvalidName("PANOSAddressGroup");
        }
    }
}
