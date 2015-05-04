namespace PANOSPsTest
{
    using NUnit.Framework;
    using PANOS;

    [TestFixture(typeof(AddressObject), "PANOSAddress", ConfigTypes.Candidate)]
    public class PsGetTests<T> : BasePsTest where T : FirewallObject
    {

        private readonly string noun;
        private readonly ConfigTypes configType;

        public PsGetTests(string noun, ConfigTypes configType)
        {
            this.noun = noun;
            this.configType = configType;
        }

        [Test]
        public void ShouldGetAll() 
        {
            // Setup - Ensure that at least 2 addresses are present
            var sut = this.RandomObjectFactory.GenerateRandomObjects<T>();
            foreach (var obj in sut)
            {
                 AddableRepository.Add(obj);
            }

            if (configType == ConfigTypes.Running)
            {
                this.CommitCandidateConfig();
            }
            
            // Test
            var script = string.Format(
                configType == ConfigTypes.Candidate ? 
                    "Get-{0} -Connection $Connection -FromCandidateConfig" :
                    "Get-{0} -Connection $Connection", noun);

            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count > 1);
            Assert.IsTrue(PsRunner.PipelineContainsFirewallObjects(results, sut));

            // Clean-up 
            foreach (var obj in sut)
            {
                this.DeletableRepository.Delete(obj.SchemaName, obj.Name);
            }
        }

        [Test]
        public void ShouldGetSingleByName() 
        {
            // Setup 
            var sut = this.RandomObjectFactory.GenerateRandomObject<T>();
            this.AddableRepository.Add(sut);
           
            if (configType == ConfigTypes.Running)
            {
                this.CommitCandidateConfig();
            }

            // Test
            var script = string.Format(
                configType == ConfigTypes.Candidate ?
                    "Get-{0} -Connection $Connection -FromCandidateConfig -Name {1}" :
                    "Get-{0} -Connection $Connection -Name {1}", noun, sut.Name );

            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(PsRunner.PipelineContainsFirewallObject(results, sut));

            // Clean-up 
            this.DeletableRepository.Delete(sut.SchemaName, sut.Name);
        }

        [Test]
        public void ShouldGetSingleByObject() 
        {
            // Setup 
            var sut = this.RandomObjectFactory.GenerateRandomObject<T>();
            this.AddableRepository.Add(sut);

            if (configType == ConfigTypes.Running)
            {
                this.CommitCandidateConfig();
            }

            // Test
            var script = string.Format(
                configType == ConfigTypes.Candidate ?
                    "$fwObject = {0}; Get-{1} -Connection $Connection -FromCandidateConfig -FirewallObject $fwObject" :
                    "$fwObject = {0}; Get-{1} -Connection $Connection -FirewallObject $fwObject", sut.ToPsScript(), noun);

            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(PsRunner.PipelineContainsFirewallObject(results, sut));

            // Clean-up 
            this.DeletableRepository.Delete(sut.SchemaName, sut.Name);
        }

        [Test]
        public void ShouldGetSingleByObjectWhenObjectsAreNotEqualButMatchOnName() 
        {
            // Setup 
            var sut = this.RandomObjectFactory.GenerateRandomObject<T>();
            this.AddableRepository.Add(sut);

            if (configType == ConfigTypes.Running)
            {
                this.CommitCandidateConfig();
            }

            // Test
            sut.Mutate();
            var script = string.Format(
                 configType == ConfigTypes.Candidate ?
                     "$fwObject = {0}; Get-{1} -Connection $Connection -FromCandidateConfig -FirewallObject $fwObject" :
                     "$fwObject = {0}; Get-{1} -Connection $Connection -FirewallObject $fwObject", sut.ToPsScript(), noun);

            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 0);
            
            // Clean-up 
            this.DeletableRepository.Delete(sut.SchemaName, sut.Name);
        }

        [Test]
        public void ShouldGetMultipleByName() 
        {
            // Setup 
            // Setup - Ensure that at least 2 addresses are present
            var sut = this.RandomObjectFactory.GenerateRandomObjects<T>();
            foreach (var obj in sut)
            {
                this.AddableRepository.Add(obj);
            }

            if (configType == ConfigTypes.Running)
            {
                this.CommitCandidateConfig();
            }

            // Test
            var script = string.Format(
                configType == ConfigTypes.Candidate ?
                    "Get-{0} -Connection $Connection -FromCandidateConfig -Name {1},{2}" :
                    "Get-{0} -Connection $Connection -Name {1},{2}", noun, sut[0].Name, sut[1].Name);

            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 2);
            Assert.IsTrue(PsRunner.PipelineContainsFirewallObjects<T>(results, sut));

            // Clean-up 
            foreach (var obj in sut)
            {
                this.DeletableRepository.Delete(obj.SchemaName, obj.Name);
            }
            
        }

        [Test]
        public void ShouldGetMultipleByNameWhereSomeDontExist() 
        {
            // Setup - Ensure that at least 2 addresses are present
            var sut = this.RandomObjectFactory.GenerateRandomObjects<T>();
            foreach (var obj in sut)
            {
                AddableRepository.Add(obj);
            }

            if (configType == ConfigTypes.Running)
            {
                CommitCandidateConfig();
            }

            // Test
            var nameBefore = sut[0].Name;
            sut[0].Name += "1";  // this should not exist
            var script = string.Format(
                configType == ConfigTypes.Candidate ?
                    "Get-{0} -Connection $Connection -FromCandidateConfig -Name {1},{2}" :
                    "Get-{0} -Connection $Connection -Name {1},{2}", noun, sut[0].Name, sut[1].Name);

            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(results[0].BaseObject is T);
            Assert.IsTrue(PsRunner.PipelineContainsFirewallObject(results, sut[1]));

            // Clean-up 
            sut[0].Name = nameBefore;
            foreach (var obj in sut)
            {
                this.DeletableRepository.Delete(obj.SchemaName, obj.Name);
            }
        }

        [Test]
        public void ShouldGetMultipleByObjectWhenSomeFailEqualsTests() 
        {
            // Setup - Ensure that at least 2 addresses are present
            var sut = this.RandomObjectFactory.GenerateRandomObjects<T>();
            foreach (var obj in sut)
            {
                this.AddableRepository.Add(obj);
            }

            if (configType == ConfigTypes.Running)
            {
                this.CommitCandidateConfig();
            }

            // Test
            
            sut[0].Mutate(); // This should not match antything
            var script = string.Format(
                configType == ConfigTypes.Candidate ?
                    "$fwObject1 = {0};$fwObject2 = {1};Get-{2} -Connection $Connection -FromCandidateConfig -FirewallObject $fwObject1, $fwObject2" :
                    "$fwObject1 = {0};$fwObject2 = {1};Get-{2} -Connection $Connection -FirewallObject $fwObject1, $fwObject2",
                        sut[0].ToPsScript(),
                        sut[1].ToPsScript(),
                        noun);

            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(results[0].BaseObject is T);
            Assert.IsTrue(PsRunner.PipelineContainsFirewallObject<T>(results, (T)sut[1]));

            // Clean-up 
            foreach (var obj in sut)
            {
                this.DeletableRepository.Delete(obj.SchemaName, obj.Name);
            }
        }

        [Test]
        public void ShouldGetMultipleByObject() 
        {
            // Setup - Ensure that at least 2 addresses are present
            var sut = this.RandomObjectFactory.GenerateRandomObjects<T>();
            foreach (var obj in sut)
            {
                this.AddableRepository.Add(obj);
            }

            if (configType == ConfigTypes.Running)
            {
                this.CommitCandidateConfig();
            }

            // Test
            var script = string.Format(
                configType == ConfigTypes.Candidate ?
                    "$fwObject1 = {0};$fwObject2 = {1};Get-{2} -Connection $Connection -FromCandidateConfig -FirewallObject $fwObject1, $fwObject2" :
                    "$fwObject1 = {0};$fwObject2 = {1};Get-{2} -Connection $Connection -FirewallObject $fwObject1, $fwObject2",
                        sut[0].ToPsScript(),
                        sut[1].ToPsScript(),
                        noun);

            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 2);
            Assert.IsTrue(results[0].BaseObject is T);
            Assert.IsTrue(results[1].BaseObject is T);
            Assert.IsTrue(PsRunner.PipelineContainsFirewallObjects<T>(results, sut));

            // Clean-up 
            foreach (var obj in sut)
            {
                this.DeletableRepository.Delete(obj.SchemaName, obj.Name);
            }
            
        }

        [Test]
        public void ShouldGetMultipleByObjectFromPipeline() 
        {
            // Setup - Ensure that at least 2 addresses are present
            var sut = this.RandomObjectFactory.GenerateRandomObjects<T>();
            foreach (var obj in sut)
            {
                this.AddableRepository.Add(obj);
            }

            if (configType == ConfigTypes.Running)
            {
                this.CommitCandidateConfig();
            }

            // Test
            var script = string.Format(
                configType == ConfigTypes.Candidate ?
                    "$fwObject1 = {0};$fwObject2 = {1};$fwObject1,$fwObject2 | Get-{2} -Connection $Connection -FromCandidateConfig" :
                    "$fwObject1 = {0};$fwObject2 = {1};$fwObject1,$fwObject2 | Get-{2} -Connection $Connection",
                        sut[0].ToPsScript(),
                        sut[1].ToPsScript(),
                        noun);

            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 2);
            Assert.IsTrue(results[0].BaseObject is T);
            Assert.IsTrue(results[1].BaseObject is T);
            Assert.IsTrue(PsRunner.PipelineContainsFirewallObjects<T>(results, sut));

            // Clean-up 
            foreach (var obj in sut)
            {
                this.DeletableRepository.Delete(obj.SchemaName, obj.Name);
            }
        }

        [Test]
        public void ShouldGetMultipleByNameFromPipeline() 
        {
            // Setup - Ensure that at least 2 addresses are present
            var sut = this.RandomObjectFactory.GenerateRandomObjects<T>();
            foreach (var obj in sut)
            {
                this.AddableRepository.Add(obj);
            }

            if (configType == ConfigTypes.Running)
            {
                this.CommitCandidateConfig();
            }

            // Test
            var script = string.Format(
                configType == ConfigTypes.Candidate ?
                    "$fwObject1 = {0};$fwObject2 = {1};$fwObject1.Name,$fwObject2.Name | Get-{2} -Connection $Connection -FromCandidateConfig" :
                    "$fwObject1 = {0};$fwObject2 = {1};$fwObject1.Name,$fwObject2.Name | Get-{2} -Connection $Connection",
                        sut[0].ToPsScript(),
                        sut[1].ToPsScript(),
                        noun);

            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 2);
            Assert.IsTrue(results[0].BaseObject is T);
            Assert.IsTrue(results[1].BaseObject is T);
            Assert.IsTrue(PsRunner.PipelineContainsFirewallObjects<T>(results, sut));

            // Clean-up 
            foreach (var obj in sut)
            {
                this.DeletableRepository.Delete(obj.SchemaName, obj.Name);
            }
        }
    }
}
