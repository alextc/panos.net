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
            var script = $"$obj = {sut.ToPsScript()};Remove-{this.noun} -{this.noun} $obj;";
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
            var script =
                $"$obj1 = {sut[0].ToPsScript()}; $obj2 = {sut[1].ToPsScript()}; Remove-{noun} -{noun} $obj1, $obj2;";
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
            var script = $"$name = '{sut.Name}';Remove-{this.noun} -Name $name;";
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
            var script = $"$name1 = '{sut[0].Name}'; $name2 = '{sut[1].Name}'; Remove-{noun} -Name $name1, $name2;";
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
            var script = $"$name1 = '{sut[0].Name}'; $name2 = '{sut[1].Name}'; $name1, $name2 | Remove-{noun};";
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
            var sut = RandomObjectFactory.GenerateRandomObjects<T>();
            foreach (var obj in sut)
            {
                AddableRepository.Add(obj);
                Assert.IsTrue(searchableRepository.GetSingle<TDeserializer>(obj.Name, ConfigTypes.Candidate).Any());
            }

            // Test
            var script =
                $"$obj1 = {sut[0].ToPsScript()}; $obj2 = {sut[1].ToPsScript()}; $obj1, $obj2 | Remove-{this.noun};";
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
            var script = $"$obj = {sut.ToPsScript()};Remove-{noun} -{noun} $obj -PassThru;";
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
            var script = $"$name = '{sut.Name}';Remove-{noun} -Name $name -PassThru;";
            var passThruObj =  psTestRunner.ExecuteCommandWithPasThru<string>(script);

            // Validate
            Assert.IsFalse(searchableRepository.GetSingle<TDeserializer>(sut.Name, ConfigTypes.Candidate).Any());

            Assert.IsNotNull(passThruObj);
            Assert.AreEqual(passThruObj, sut.Name);
        }
    }
}
