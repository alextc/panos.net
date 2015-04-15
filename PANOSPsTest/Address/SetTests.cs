namespace PANOSPsTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using PANOS;

    using PANOSLibTest;

    [TestClass]
    public class SetTests : BaseConfigTest
    {
        private readonly PsSetTests psSetTests = new PsSetTests();
        [TestMethod]
        public void SetSingleAddressFromObjectPassedAsParameter()
        {
            Assert.IsTrue(this.psSetTests.SetSingleFromObjectPassedAsParameter<GetSingleAddressApiResponse, AddressObject>());
            Assert.IsTrue(this.psSetTests.SetSingleFromObjectPassedAsParameter<GetSingleAddressApiResponse, SubnetObject>());
            Assert.IsTrue(this.psSetTests.SetSingleFromObjectPassedAsParameter<GetSingleAddressApiResponse, AddressRangeObject>());
            Assert.IsTrue(this.psSetTests.SetSingleFromObjectPassedAsParameter<GetSingleAddressGroupApiResponse, AddressGroupObject>());
        }
        
        [TestMethod]
        public void SetSMultipleAddressesFromObjectsPassedAsParameter()
        {
            Assert.IsTrue(this.psSetTests.SetMultipleFromObjectPassedAsParameter<GetSingleAddressApiResponse, AddressObject>());
            Assert.IsTrue(this.psSetTests.SetMultipleFromObjectPassedAsParameter<GetSingleAddressApiResponse, SubnetObject>());
            Assert.IsTrue(this.psSetTests.SetMultipleFromObjectPassedAsParameter<GetSingleAddressApiResponse, AddressRangeObject>());
            Assert.IsTrue(this.psSetTests.SetMultipleFromObjectPassedAsParameter<GetSingleAddressGroupApiResponse, AddressGroupObject>());
        }
        
        [TestMethod]
        public void SetSMultipleAddressesFromObjectsPassedFromPipeLine()
        {
            Assert.IsTrue(this.psSetTests.SetMultipleFromObjectPassedFromPipeline<GetSingleAddressApiResponse, AddressObject>());
            Assert.IsTrue(this.psSetTests.SetMultipleFromObjectPassedFromPipeline<GetSingleAddressApiResponse, SubnetObject>());
            Assert.IsTrue(this.psSetTests.SetMultipleFromObjectPassedFromPipeline<GetSingleAddressApiResponse, AddressRangeObject>());
            Assert.IsTrue(this.psSetTests.SetMultipleFromObjectPassedFromPipeline<GetSingleAddressGroupApiResponse, AddressGroupObject>());
        }
    }
}
