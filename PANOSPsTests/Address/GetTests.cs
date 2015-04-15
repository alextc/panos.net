namespace PANOSPsTest
{
    using System.Management.Automation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PANOS;
    using PANOSLibTest;

    [TestClass]
    public class GetTests : BaseConfigTest
    {
        private readonly PsGetTests psGetTests = new PsGetTests();

        [TestMethod]
        public void GetAllAddressesFromCandidateConfig()
        {
            Assert.IsTrue(this.psGetTests.GetAll<AddressObject>("PANOSAddress", ConfigTypes.Candidate));
            Assert.IsTrue(this.psGetTests.GetAll<SubnetObject>("PANOSSubnet", ConfigTypes.Candidate));
            Assert.IsTrue(this.psGetTests.GetAll<AddressRangeObject>("PANOSAddressRange", ConfigTypes.Candidate));
            Assert.IsTrue(this.psGetTests.GetAll<AddressGroupObject>("PANOSAddressGroup", ConfigTypes.Candidate));
        }

        // [TestMethod]
        public void GetAllAddressesFromRunningConfig()
        {
            Assert.IsTrue(this.psGetTests.GetAll<AddressObject>("PANOSAddress", ConfigTypes.Running));
            Assert.IsTrue(this.psGetTests.GetAll<SubnetObject>("PANOSSubnet", ConfigTypes.Running));
            Assert.IsTrue(this.psGetTests.GetAll<AddressRangeObject>("PANOSAddressRange", ConfigTypes.Running));
            Assert.IsTrue(this.psGetTests.GetAll<AddressGroupObject>("PANOSAddressGroup", ConfigTypes.Running));
        }

        // [TestMethod]
        public void GetSingleAddressByNameFromRunning()
        {
            Assert.IsTrue(this.psGetTests.GetSingleByName<AddressObject>("PANOSAddress", ConfigTypes.Running));
            Assert.IsTrue(this.psGetTests.GetSingleByName<SubnetObject>("PANOSSubnet", ConfigTypes.Running));
            Assert.IsTrue(this.psGetTests.GetSingleByName<AddressRangeObject>("PANOSAddressRange", ConfigTypes.Running));
            Assert.IsTrue(this.psGetTests.GetSingleByName<AddressGroupObject>("PANOSAddressGroup", ConfigTypes.Running));
        }

        [TestMethod]
        public void GetSingleAddressByNameFromCandiate()
        {
            Assert.IsTrue(this.psGetTests.GetSingleByName<AddressObject>("PANOSAddress", ConfigTypes.Candidate));
            Assert.IsTrue(this.psGetTests.GetSingleByName<SubnetObject>("PANOSSubnet", ConfigTypes.Candidate));
            Assert.IsTrue(this.psGetTests.GetSingleByName<AddressRangeObject>("PANOSAddressRange", ConfigTypes.Candidate));
            Assert.IsTrue(this.psGetTests.GetSingleByName<AddressGroupObject>("PANOSAddressGroup", ConfigTypes.Candidate));
        }
        
        [TestMethod]
        public void GetSingleAddressByObjectFromCandidate()
        {
            Assert.IsTrue(this.psGetTests.GetSingleByObject<AddressObject>("PANOSAddress", ConfigTypes.Candidate));
            Assert.IsTrue(this.psGetTests.GetSingleByObject<SubnetObject>("PANOSSubnet", ConfigTypes.Candidate));
            Assert.IsTrue(this.psGetTests.GetSingleByObject<AddressRangeObject>("PANOSAddressRange", ConfigTypes.Candidate));
            Assert.IsTrue(this.psGetTests.GetSingleByObject<AddressGroupObject>("PANOSAddressGroup", ConfigTypes.Candidate));
        }

        // [TestMethod]
        public void GetSingleAddressByObjectFromRunning()
        {
            Assert.IsTrue(this.psGetTests.GetSingleByObject<AddressObject>("PANOSAddress", ConfigTypes.Running));
            Assert.IsTrue(this.psGetTests.GetSingleByObject<SubnetObject>("PANOSSubnet", ConfigTypes.Running));
            Assert.IsTrue(this.psGetTests.GetSingleByObject<AddressRangeObject>("PANOSAddressRange", ConfigTypes.Running));
            Assert.IsTrue(this.psGetTests.GetSingleByObject<AddressGroupObject>("PANOSAddressGroup", ConfigTypes.Running));
        }
        
        [TestMethod]
        public void GetSingleAddressByObjectWhereObjectsAreNotEqualButMatchOnName()
        {
            Assert.IsTrue(this.psGetTests.GetSingleByObjectWhereObjectsAreNotEqualButMatchOnName<AddressObject>("PANOSAddress", ConfigTypes.Candidate));
            Assert.IsTrue(this.psGetTests.GetSingleByObjectWhereObjectsAreNotEqualButMatchOnName<SubnetObject>("PANOSSubnet", ConfigTypes.Candidate));
            Assert.IsTrue(this.psGetTests.GetSingleByObjectWhereObjectsAreNotEqualButMatchOnName<AddressRangeObject>("PANOSAddressRange", ConfigTypes.Candidate));
            Assert.IsTrue(this.psGetTests.GetSingleByObjectWhereObjectsAreNotEqualButMatchOnName<AddressGroupObject>("PANOSAddressGroup", ConfigTypes.Candidate));
        }
        
        [TestMethod]
        public void GetMultipleAddressesByName()
        {
            Assert.IsTrue(this.psGetTests.GetMultipleByName<AddressObject>("PANOSAddress", ConfigTypes.Candidate));
            Assert.IsTrue(this.psGetTests.GetMultipleByName<SubnetObject>("PANOSSubnet", ConfigTypes.Candidate));
            Assert.IsTrue(this.psGetTests.GetMultipleByName<AddressRangeObject>("PANOSAddressRange", ConfigTypes.Candidate));
            Assert.IsTrue(this.psGetTests.GetMultipleByName<AddressGroupObject>("PANOSAddressGroup", ConfigTypes.Candidate));
        }
        
       [TestMethod]
       public void GetMultipleAddressesByNameWhereSomeDoNotExist()
       {
           Assert.IsTrue(this.psGetTests.GetMultipleByNameWhereSomeDontExist<AddressObject>("PANOSAddress", ConfigTypes.Candidate));
           Assert.IsTrue(this.psGetTests.GetMultipleByNameWhereSomeDontExist<SubnetObject>("PANOSSubnet", ConfigTypes.Candidate));
           Assert.IsTrue(this.psGetTests.GetMultipleByNameWhereSomeDontExist<AddressRangeObject>("PANOSAddressRange", ConfigTypes.Candidate));
           Assert.IsTrue(this.psGetTests.GetMultipleByNameWhereSomeDontExist<AddressGroupObject>("PANOSAddressGroup", ConfigTypes.Candidate));
       }

       [TestMethod]
       public void GetMultipleAddressesByObject()
       {
           Assert.IsTrue(this.psGetTests.GetMultipleByObjectWhereSomeFailEqualsTests<AddressObject>("PANOSAddress", ConfigTypes.Candidate));
           Assert.IsTrue(this.psGetTests.GetMultipleByObjectWhereSomeFailEqualsTests<SubnetObject>("PANOSSubnet", ConfigTypes.Candidate));
           Assert.IsTrue(this.psGetTests.GetMultipleByObjectWhereSomeFailEqualsTests<AddressRangeObject>("PANOSAddressRange", ConfigTypes.Candidate));
           Assert.IsTrue(this.psGetTests.GetMultipleByObjectWhereSomeFailEqualsTests<AddressGroupObject>("PANOSAddressGroup", ConfigTypes.Candidate));
       }

       [TestMethod]
       public void GetMultipleAddressesByObjectWhereOneFailsEqualTest()
       {
           Assert.IsTrue(this.psGetTests.GetMultipleByObject<AddressObject>("PANOSAddress", ConfigTypes.Candidate));
           Assert.IsTrue(this.psGetTests.GetMultipleByObject<SubnetObject>("PANOSSubnet", ConfigTypes.Candidate));
           Assert.IsTrue(this.psGetTests.GetMultipleByObject<AddressRangeObject>("PANOSAddressRange", ConfigTypes.Candidate));
           Assert.IsTrue(this.psGetTests.GetMultipleByObject<AddressGroupObject>("PANOSAddressGroup", ConfigTypes.Candidate));
       }

       [TestMethod]
       public void GetMultipleAddressesByObjectViaPipeLine()
       {
           Assert.IsTrue(this.psGetTests.GetMultipleByObjectFromPipeline<AddressObject>("PANOSAddress", ConfigTypes.Candidate));
           Assert.IsTrue(this.psGetTests.GetMultipleByObjectFromPipeline<SubnetObject>("PANOSSubnet", ConfigTypes.Candidate));
           Assert.IsTrue(this.psGetTests.GetMultipleByObjectFromPipeline<AddressRangeObject>("PANOSAddressRange", ConfigTypes.Candidate));
           Assert.IsTrue(this.psGetTests.GetMultipleByObjectFromPipeline<AddressGroupObject>("PANOSAddressGroup", ConfigTypes.Candidate));
       }

       [TestMethod]
       public void GetMultipleAddressesByNameViaPipeLine()
       {
           Assert.IsTrue(this.psGetTests.GetMultipleByObjectFromPipeline<AddressObject>("PANOSAddress", ConfigTypes.Candidate));
           Assert.IsTrue(this.psGetTests.GetMultipleByObjectFromPipeline<SubnetObject>("PANOSSubnet", ConfigTypes.Candidate));
           Assert.IsTrue(this.psGetTests.GetMultipleByObjectFromPipeline<AddressRangeObject>("PANOSAddressRange", ConfigTypes.Candidate));
           Assert.IsTrue(this.psGetTests.GetMultipleByObjectFromPipeline<AddressGroupObject>("PANOSAddressGroup", ConfigTypes.Candidate));
       }

        [TestMethod]
        [ExpectedException(typeof(ParameterBindingException), AllowDerivedTypes = true)]
        public void RejectInvalidNameParameterTest()
        {
            this.psGetTests.RejectInvalidName("PANOSAddress");
            this.psGetTests.RejectInvalidName("PANOSSubnet");
            this.psGetTests.RejectInvalidName("PANOSAddressRange");
            this.psGetTests.RejectInvalidName("PANOSAddressGroup");
        }
    }
}
