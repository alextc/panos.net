namespace PANOSLibTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using PANOS;

    [TestClass]
    public class SetAddressTests 
    {
        private readonly SetTests baseSetTests = new SetTests();
        [TestMethod]
        public void AddNewAddressTest()
        {
            Assert.IsTrue(this.baseSetTests.AddObject<GetSingleAddressApiResponse, AddressObject>(Schema.AddressSchemaName));
            Assert.IsTrue(this.baseSetTests.AddObject<GetSingleAddressApiResponse, SubnetObject>(Schema.AddressSchemaName));
            Assert.IsTrue(this.baseSetTests.AddObject<GetSingleAddressApiResponse, AddressRangeObject>(Schema.AddressSchemaName));
            Assert.IsTrue(this.baseSetTests.AddObject<GetSingleAddressGroupApiResponse, AddressGroupObject>(Schema.AddressGroupSchemaName));
        }
    }
}
