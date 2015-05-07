namespace PANOSPsTest
{
    using System.Linq;
    using NUnit.Framework;
    using PANOS;

    [TestFixture(typeof(AddressObject), typeof(GetSingleAddressApiResponse), "Add", "PANOSAddress", Schema.AddressSchemaName)]
    [TestFixture(typeof(AddressObject), typeof(GetSingleAddressApiResponse), "Set", "PANOSAddress", Schema.AddressSchemaName)]
    public class PsAddSetTests<T, TDeserializer> : BasePsTest
        where TDeserializer : ApiResponseForGetSingle
        where T : FirewallObject
    {
        // TODO: Test to simulate a failure from PANOS during addition.
        // Not sure how to inject a fault at the moment, perhaps illegal character in the name?

        private readonly string verb;
        private readonly string noun;
        private readonly PsTestRunner<T> psTestRunner;
        private readonly ISearchableRepository<T> searchableRepository;

        public PsAddSetTests(string verb, string noun, string schemaName)
        {
            this.verb = verb;
            this.noun = noun;
            psTestRunner = new PsTestRunner<T>();
            searchableRepository = new SearchableRepository<T>(ConfigCommandFactory, schemaName);
        }

        [Test]
        public void ShouldAddSingleFromObjectPassedAsParameter()    
        {
            // Setup
            var sut = RandomObjectFactory.GenerateRandomObject<T>();
            
            // Test
            var script = string.Format(
                "$obj = {0};{1}-{2} -{3} $obj", sut.ToPsScript(), verb, noun, noun);
            psTestRunner.ExecuteCommand(script);

            // Validate
            var confirmationObject = searchableRepository.GetSingle<TDeserializer>(sut.Name, ConfigTypes.Candidate);
            Assert.IsTrue(confirmationObject.Any());
            Assert.AreEqual(sut, confirmationObject.Single());

            // Cleanup
            DeletableRepository.Delete(sut.SchemaName, sut.Name);
        }

        [Test]
        public void ShouldPassThruCreatedObject()
        {
            // Setup
            var sut = RandomObjectFactory.GenerateRandomObject<T>();
            
            // Test
            var script = string.Format(
                "$obj = {0};{1}-{2} -{3} $obj -PassThru", sut.ToPsScript(), verb, noun, noun);
            var passedThruObj = psTestRunner.ExecuteCommandWithPasThru(script);

            // Validate
            Assert.IsNotNull(passedThruObj);
            Assert.AreEqual(passedThruObj, sut);

            var confirmationObject = searchableRepository.GetSingle<TDeserializer>(sut.Name, ConfigTypes.Candidate);
            Assert.IsTrue(confirmationObject.Any());
            Assert.AreEqual(sut, confirmationObject.Single());

            // Cleanup
            DeletableRepository.Delete(sut.SchemaName, sut.Name);
        }

        [Test]
        public void ShouldSetMultipleFromObjectPassedAsParameter()
        {
            // Setup
            var sut = RandomObjectFactory.GenerateRandomObjects<T>();
            
            // Test
            var script = string.Format(
                "$obj1 = {0};$obj2={1};{2}-{3} -{4} $obj1,$obj2",
                    sut[0].ToPsScript(),
                    sut[1].ToPsScript(),
                    verb,
                    noun, 
                    noun);
            psTestRunner.ExecuteCommand(script);

            // Validate
            foreach (var obj in sut)
            {
                var confirmationObject = searchableRepository.GetSingle<TDeserializer>(obj.Name, ConfigTypes.Candidate);
                Assert.IsTrue(confirmationObject.Any());
                Assert.AreEqual(obj, confirmationObject.Single());
            }
            
            // Cleanup
            foreach (var obj in sut)
            {
                DeletableRepository.Delete(obj.SchemaName, obj.Name);
            }
        }

        [Test]
        public void ShouldSetMultipleFromObjectPassedFromPipeline()
        {
            // Setup
            var sut = RandomObjectFactory.GenerateRandomObjects<T>();
            
            // Test
            var script = string.Format(
                "$obj1 = {0};$obj2={1}; $obj1,$obj2 | {2}-{3}",
                    sut[0].ToPsScript(),
                    sut[1].ToPsScript(),
                    verb,
                    noun);
            PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            foreach (var obj in sut)
            {
                var confirmationObject = this.searchableRepository.GetSingle<TDeserializer>(obj.Name, ConfigTypes.Candidate);
                Assert.IsTrue(confirmationObject.Any());
                Assert.AreEqual(obj, confirmationObject.Single());
            }
            
            // Cleanup
            foreach (var obj in sut)
            {
                DeletableRepository.Delete(obj.SchemaName, obj.Name);
            }
        }
    }
}
