namespace PANOSPsTest
{
    using System.Linq;
    using NUnit.Framework;
    using PANOS;

    [TestFixture(typeof(AddressObject), typeof(GetSingleAddressApiResponse), "PANOSAddress", Schema.AddressSchemaName)]
    public class PsDeleteTests<T, TDeserializer> : BasePsTest
        where TDeserializer : ApiResponseForGetSingle
        where T : FirewallObject
    {
        private readonly string noun;
        private readonly PsTestRunner<T> psTestRunner;
        private readonly ISearchableRepository<T> searchableRepository;
        
        public PsDeleteTests(string noun, string schemaName)
        {
            this.noun = noun;
            psTestRunner = new PsTestRunner<T>();
            searchableRepository = new SearchableRepository<T>(ConfigCommandFactory, schemaName);
        }

        [Test]
        public void ShouldDeleteSingleByObjectPassedAsParameter()
        {
            // Setup
            var sut = RandomObjectFactory.GenerateRandomObject<T>();
            this.AddableRepository.Add(sut);
            Assert.IsTrue(searchableRepository.GetSingle<TDeserializer>(sut.Name, ConfigTypes.Candidate).Any());

            // Test
            var script = string.Format("$obj = {0};Remove-{1} -{2} $obj;", sut.ToPsScript(), noun, noun);
            psTestRunner.ExecuteCommand(script);

            // Validate
            Assert.IsFalse(searchableRepository.GetSingle<TDeserializer>(sut.Name, ConfigTypes.Candidate).Any());
            
        }

        [Test]
        public void ShouldDeleteMultipleByObjectPassedAsParameter()
        {
            // Setup
            var sut = RandomObjectFactory.GenerateRandomObjects<T>();
            foreach (var obj in sut)
            {
                AddableRepository.Add(obj);
                Assert.IsTrue(searchableRepository.GetSingle<TDeserializer>(obj.Name, ConfigTypes.Candidate).Any());
            }
            
            // Test
            var script = string.Format("$obj1 = {0}; $obj2 = {1}; Remove-{2} -{3} $obj1, $obj2;", sut[0].ToPsScript(), sut[1].ToPsScript(), noun, noun);
            psTestRunner.ExecuteCommand(script);

            // Validate
            foreach (var obj in sut)
            {
                Assert.IsFalse(searchableRepository.GetSingle<TDeserializer>(obj.Name, ConfigTypes.Candidate).Any());
            }
        }

        [Test]
        public void ShouldDeleteSingleByNamePassedAsParameter()
        {
            // Setup
            var sut = RandomObjectFactory.GenerateRandomObject<T>();
            AddableRepository.Add(sut);
            Assert.IsTrue(searchableRepository.GetSingle<TDeserializer>(sut.Name, ConfigTypes.Candidate).Any());

            // Test
            var script = string.Format("$name = '{0}';Remove-{1} -Name $name;", sut.Name, noun);
            psTestRunner.ExecuteCommand(script);

            // Validate
            Assert.IsFalse(searchableRepository.GetSingle<TDeserializer>(sut.Name, ConfigTypes.Candidate).Any());
        }

        [Test]
        public void ShouldDeleteMultipleByNamePassedAsParameter()
        {
            // Setup
            var sut = RandomObjectFactory.GenerateRandomObjects<T>();
            foreach (var obj in sut)
            {
                AddableRepository.Add(obj);
                Assert.IsTrue(searchableRepository.GetSingle<TDeserializer>(obj.Name, ConfigTypes.Candidate).Any());
            }

            // Test
            var script = string.Format("$name1 = '{0}'; $name2 = '{1}'; Remove-{2} -Name $name1, $name2;", sut[0].Name, sut[1].Name, noun);
            psTestRunner.ExecuteCommand(script);

            // Validate
            foreach (var obj in sut)
            {
                Assert.IsFalse(searchableRepository.GetSingle<TDeserializer>(obj.Name, ConfigTypes.Candidate).Any());
            }
        }

        [Test]
        public void ShouldDeleteMultipleByNamePassedViaPipeline()
        {
            // Setup
            var sut = RandomObjectFactory.GenerateRandomObjects<T>();
            foreach (var obj in sut)
            {
                AddableRepository.Add(obj);
                Assert.IsTrue(searchableRepository.GetSingle<TDeserializer>(obj.Name, ConfigTypes.Candidate).Any());
            }

            // Test
            var script = string.Format("$name1 = '{0}'; $name2 = '{1}'; $name1, $name2 | Remove-{2};",sut[0].Name, sut[1].Name, noun);
            psTestRunner.ExecuteCommand(script);

            // Validate
            foreach (var obj in sut)
            {
                Assert.IsFalse(searchableRepository.GetSingle<TDeserializer>(obj.Name, ConfigTypes.Candidate).Any());
            }
        }

        [Test]
        public void ShouldDeleteMultipleByObjectPassedViaPipeline()
        {
            // Setup
            var sut = this.RandomObjectFactory.GenerateRandomObjects<T>();
            foreach (var obj in sut)
            {
                AddableRepository.Add(obj);
                Assert.IsTrue(searchableRepository.GetSingle<TDeserializer>(obj.Name, ConfigTypes.Candidate).Any());
            }

            // Test
            var script =
                string.Format("$obj1 = {0}; $obj2 = {1}; $obj1, $obj2 | Remove-{2};", sut[0].ToPsScript(), sut[1].ToPsScript(), noun);
            psTestRunner.ExecuteCommand(script);

            // Validate
            foreach (var obj in sut)
            {
                Assert.IsFalse(searchableRepository.GetSingle<TDeserializer>(obj.Name, ConfigTypes.Candidate).Any());
            }
        }

        [Test]
        public void ShouldDeleteAndPassThruObjectTest()
        {
            // Setup
            var sut = RandomObjectFactory.GenerateRandomObject<T>();
            AddableRepository.Add(sut);
            Assert.IsTrue(searchableRepository.GetSingle<TDeserializer>(sut.Name, ConfigTypes.Candidate).Any());

            // Test
            var script = string.Format("$obj = {0};Remove-{1} -{2} $obj -PassThru;", sut.ToPsScript(), noun, noun);
            var passThruObject =  psTestRunner.ExecuteCommandWithPasThru(script);

            // Validate
            Assert.IsFalse(searchableRepository.GetSingle<TDeserializer>(sut.Name, ConfigTypes.Candidate).Any());
            Assert.IsNotNull(passThruObject);
            Assert.AreEqual(passThruObject, sut);  
        }

        [Test]
        public void ShouldDeleteAndPassThruName()
        {
            // Setup
            var sut = RandomObjectFactory.GenerateRandomObject<T>();
            AddableRepository.Add(sut);
            Assert.IsTrue(searchableRepository.GetSingle<TDeserializer>(sut.Name, ConfigTypes.Candidate).Any());

            // Test
            var script = string.Format("$name = '{0}';Remove-{1} -Name $name -PassThru;", sut.Name, noun);
            var passThruObj =  psTestRunner.ExecuteCommandWithPasThru<string>(script);

            // Validate
            Assert.IsFalse(searchableRepository.GetSingle<TDeserializer>(sut.Name, ConfigTypes.Candidate).Any());

            Assert.IsNotNull(passThruObj);
            Assert.AreEqual(passThruObj, sut.Name);
        }
    }
}
