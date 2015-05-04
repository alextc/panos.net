namespace PANOSPsTest
{
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using PANOS;

    [TestFixture(typeof(AddressObject), "PANOSAddress", ConfigTypes.Candidate)]
    public class PsGetTests<T> : BasePsTest where T : FirewallObject
    {

        private readonly string noun;
        private readonly ConfigTypes configType;
        private List<T> sut;
        private readonly string command;

        public PsGetTests(string noun, ConfigTypes configType)
        {
            this.noun = noun;
            this.configType = configType;
            command = string.Format(
                configType == ConfigTypes.Candidate ?
                    "Get-{0} -Connection $Connection -FromCandidateConfig" :
                    "Get-{0} -Connection $Connection", noun);
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
            //Test
            var results = PsRunner.ExecutePanosPowerShellScript(command);

            // Validate
            Assert.IsTrue(results.Count > 1);
            Assert.IsTrue(PsRunner.PipelineContainsFirewallObjects(results, sut));
        }

        [Test]
        public void ShouldGetSingleByName() 
        {
            // Test
            var script = string.Format("{0} -Name {1}", command, sut.First().Name );
            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(PsRunner.PipelineContainsFirewallObject(results, sut.First()));
        }

        [Test]
        public void ShouldGetSingleByObject() 
        {
            // Test
            var script = string.Format("$fwObject = {0};{1} -FirewallObject $fwObject", sut.First().ToPsScript(), command);
            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(PsRunner.PipelineContainsFirewallObject(results, sut.First()));
        }

        [Test]
        public void ShouldNotGetSingleWhenDeepCompareFails() 
        {
            // Test
            var temp = sut.First();
            temp.Mutate();
            var script = string.Format("$fwObject = {0};{1} -FirewallObject $fwObject", temp.ToPsScript(), command);
            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 0);
        }

        [Test]
        public void ShouldGetMultipleByName() 
        {
            // Test
            var script = string.Format(command + " -Name {0},{1}", sut[0].Name, sut[1].Name);
            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 2);
            Assert.IsTrue(PsRunner.PipelineContainsFirewallObjects(results, sut));
        }

        [Test]
        public void ShouldNotGetSingleByNameWhenNameDoesNotExist() 
        {
            // Test
            var nonExistingName = sut.First().Name;
            nonExistingName += "1";  // this should not exist
            var script = string.Format("{0} -Name {1}", command, nonExistingName);
            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 0);
        }
        
        [Test]
        public void ShouldGetMultipleByObjectArray() 
        {
            // Test
            var script = string.Format("$fwObject1 = {0};$fwObject2 = {1};" +  command +  " -FirewallObject $fwObject1, $fwObject2", sut[0].ToPsScript(), sut[1].ToPsScript());
            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 2);
            Assert.IsTrue(results[0].BaseObject is T);
            Assert.IsTrue(results[1].BaseObject is T);
            Assert.IsTrue(PsRunner.PipelineContainsFirewallObjects(results, sut));
        }

        [Test]
        public void ShouldGetMultipleByObjectFromPipeline() 
        {
            // Test
            var script = string.Format("$fwObject1 = {0};$fwObject2 = {1};$fwObject1,$fwObject2 | " + command, sut[0].ToPsScript(), sut[1].ToPsScript());
            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 2);
            Assert.IsTrue(results[0].BaseObject is T);
            Assert.IsTrue(results[1].BaseObject is T);
            Assert.IsTrue(PsRunner.PipelineContainsFirewallObjects<T>(results, sut));
        }

        [Test]
        public void ShouldGetMultipleByNameFromPipeline() 
        {
            // Test
            var script = string.Format("'{0}','{1}' | {2}", sut[0].Name, sut[1].Name, command);
            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 2);
            Assert.IsTrue(results[0].BaseObject is T);
            Assert.IsTrue(results[1].BaseObject is T);
            Assert.IsTrue(PsRunner.PipelineContainsFirewallObjects<T>(results, sut));
        }
    }
}
