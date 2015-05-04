namespace PANOSPsTest
{
    using NUnit.Framework;

    using PANOS;

    [TestFixture]
    public class RenameTests 
    {
        private readonly PsRenameTests renameTests = new PsRenameTests();

        [Test]
        public void RenameSingleAddressFromObjectPassedAsParameter()
        {
            Assert.IsTrue(this.renameTests.RenameSingleFromObjectPassedAsParameter<GetSingleAddressApiResponse, AddressObject>());
            Assert.IsTrue(this.renameTests.RenameSingleFromObjectPassedAsParameter<GetSingleAddressApiResponse, SubnetObject>());
            Assert.IsTrue(this.renameTests.RenameSingleFromObjectPassedAsParameter<GetSingleAddressApiResponse, AddressRangeObject>());
            Assert.IsTrue(this.renameTests.RenameSingleFromObjectPassedAsParameter<GetSingleAddressGroupApiResponse, AddressGroupObject>());
        }

        [Test]
        public void RenameSingleAddressFromNamePassedAsParameter()
        {
            Assert.IsTrue(this.renameTests.RenameSingleFromNamePassedAsParameter<GetSingleAddressApiResponse, AddressObject>());
            Assert.IsTrue(this.renameTests.RenameSingleFromNamePassedAsParameter<GetSingleAddressApiResponse, SubnetObject>());
            Assert.IsTrue(this.renameTests.RenameSingleFromNamePassedAsParameter<GetSingleAddressApiResponse, AddressRangeObject>());
            Assert.IsTrue(this.renameTests.RenameSingleFromNamePassedAsParameter<GetSingleAddressGroupApiResponse, AddressGroupObject>());
        }

        [Test]
        public void RenameMultipleAddressesFromObjectsPassedViaPipeline()
        {
            Assert.IsTrue(this.renameTests.RenameMultipleFromObjectsPassedViaPipeline<GetSingleAddressApiResponse, AddressObject>());
            Assert.IsTrue(this.renameTests.RenameMultipleFromObjectsPassedViaPipeline<GetSingleAddressApiResponse, SubnetObject>());
            Assert.IsTrue(this.renameTests.RenameMultipleFromObjectsPassedViaPipeline<GetSingleAddressApiResponse, AddressRangeObject>());
            Assert.IsTrue(this.renameTests.RenameMultipleFromObjectsPassedViaPipeline<GetSingleAddressGroupApiResponse, AddressGroupObject>());
        }
    }
}
