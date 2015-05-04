namespace PANOSPsTest
{
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using PANOS;

    [TestFixture(typeof(AddressObject), "PANOSAddress", ConfigTypes.Candidate)]
    public class PsGetTests<T> : BasePsTest where T : FirewallObject
    {
        private readonly ConfigTypes configType;
        private List<T> sut;
        private readonly string command;

        private readonly PsTestRunner<T> psTestRunner;

        public PsGetTests(string noun, ConfigTypes configType)
        {
            this.configType = configType;
            command = string.Format(configType == ConfigTypes.Candidate ? "Get-{0} -FromCandidateConfig" : "Get-{0}", noun);
            psTestRunner = new PsTestRunner<T>();
        }

        [TestFixtureSetUp]
        public void Setup()
        {
            this.sut = new List<T>
            {
                RandomObjectFactory.GenerateRandomObject<T>(),
                RandomObjectFactory.GenerateRandomObject<T>()
            };

            foreach (var objectUnderTest in this.sut)
            {
                AddableRepository.Add(objectUnderTest);
            }

            if (configType == ConfigTypes.Running)
            {
                CommitCandidateConfig();
            }
        }

        [TestFixtureTearDown]
        public void CleanUp()
        {
            foreach (var obj in this.sut)
            {
                DeletableRepository.Delete(obj.SchemaName, obj.Name);

                // If this is a group object, delete its members
                // TODO: Deal with nested Groups
                if (obj is AddressGroupObject)
                {
                    foreach (var member in (obj as AddressGroupObject).Members)
                    {
                        DeletableRepository.Delete(Schema.AddressSchemaName, member);
                    }
                }
            }
        }

        [Test]
        public void ShouldGetAll() 
        {  
            Assert.IsTrue(psTestRunner.ExecuteQuery(command).Count > 1);
        }

        [Test]
        public void ShouldGetSingleByName() 
        {
            var script = string.Format("{0} -Name {1}", command, sut.First().Name);
            var result = psTestRunner.ExecuteQuery(script);
            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result.Single().Equals(sut.First()));
        }

        [Test]
        public void ShouldGetSingleByObject() 
        {
            var script = string.Format("$fwObject = {0};{1} -FirewallObject $fwObject", sut.First().ToPsScript(), command);
            var result = psTestRunner.ExecuteQuery(script);
            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result.Single().Equals(sut.First()));
        }

        [Test]
        public void ShouldNotGetSingleWhenDeepCompareFails() 
        {
            var temp = sut.First();
            temp.Mutate();
            var script = string.Format("$fwObject = {0};{1} -FirewallObject $fwObject", temp.ToPsScript(), command);
            Assert.IsTrue(psTestRunner.ExecuteQuery(script).Count == 0);
        }

        [Test]
        public void ShouldGetMultipleByName() 
        {
            var script = string.Format(command + " -Name {0},{1}", sut[0].Name, sut[1].Name);
            var result = psTestRunner.ExecuteQuery(script);
            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(result[0].Equals(sut[0]));
            Assert.IsTrue(result[1].Equals(sut[1]));
        }

        [Test]
        public void ShouldNotGetSingleByNameWhenNameDoesNotExist() 
        {
            // Test
            var nonExistingName = sut.First().Name;
            nonExistingName += "1";  // this should not exist
            var script = string.Format("{0} -Name {1}", command, nonExistingName);
            Assert.IsTrue(psTestRunner.ExecuteQuery(script).Count == 0);
        }
        
        [Test]
        public void ShouldGetMultipleByObjectArray() 
        {
            var script = string.Format("$fwObject1 = {0};$fwObject2 = {1};" +  command +  " -FirewallObject $fwObject1, $fwObject2", sut[0].ToPsScript(), sut[1].ToPsScript());
            var result = psTestRunner.ExecuteQuery(script);
            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(result[0].Equals(sut[0]));
            Assert.IsTrue(result[1].Equals(sut[1]));
        }

        [Test]
        public void ShouldGetMultipleByObjectFromPipeline() 
        {
            var script = string.Format("$fwObject1 = {0};$fwObject2 = {1};$fwObject1,$fwObject2 | " + command, sut[0].ToPsScript(), sut[1].ToPsScript());
            var result = psTestRunner.ExecuteQuery(script);
            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(result[0].Equals(sut[0]));
            Assert.IsTrue(result[1].Equals(sut[1]));
        }

        [Test]
        public void ShouldGetMultipleByNameFromPipeline() 
        {
            // Test
            var script = string.Format("'{0}','{1}' | {2}", sut[0].Name, sut[1].Name, command);
            var result = psTestRunner.ExecuteQuery(script);
            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(result[0].Equals(sut[0]));
            Assert.IsTrue(result[1].Equals(sut[1]));
        }
    }
}
