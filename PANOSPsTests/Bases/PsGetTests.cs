namespace PANOSPsTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PANOS;
    using PANOSLibTest;

    public class PsGetTests : BaseConfigTest
    {
        public bool GetAll<T>(string noun, ConfigTypes configType) where T: FirewallObject
        {
            // Setup - Ensure that at least 2 addresses are present
            var objectsUnderTest = this.RandomObjectFactory.GenerateRandomObjects<T>();
            foreach (var obj in objectsUnderTest)
            {
                this.ConfigRepository.Set(obj);
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
            Assert.IsTrue(PsRunner.PipelineContainsFirewallObjects<T>(results, objectsUnderTest));

            // Clean-up 
            foreach (var obj in objectsUnderTest)
            {
                this.ConfigRepository.Delete(obj.SchemaName, obj.Name);
            }
            return true;
        }

        public bool GetSingleByName<T>(string noun, ConfigTypes configType) where T : FirewallObject
        {
            // Setup 
            var objectUnderTest = this.RandomObjectFactory.GenerateRandomObject<T>();
            this.ConfigRepository.Set(objectUnderTest);
           
            if (configType == ConfigTypes.Running)
            {
                this.CommitCandidateConfig();
            }

            // Test
            var script = string.Format(
                configType == ConfigTypes.Candidate ?
                    "Get-{0} -Connection $Connection -FromCandidateConfig -Name {1}" :
                    "Get-{0} -Connection $Connection -Name {1}", noun, objectUnderTest.Name );

            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(PsRunner.PipelineContainsFirewallObject<T>(results, objectUnderTest));

            // Clean-up 
            this.ConfigRepository.Delete(objectUnderTest.SchemaName, objectUnderTest.Name);
            
            return true;
        }

        public bool GetSingleByObject<T>(string noun, ConfigTypes configType) where T : FirewallObject
        {
            // Setup 
            var objectUnderTest = this.RandomObjectFactory.GenerateRandomObject<T>();
            this.ConfigRepository.Set(objectUnderTest);

            if (configType == ConfigTypes.Running)
            {
                this.CommitCandidateConfig();
            }

            // Test
            var script = string.Format(
                configType == ConfigTypes.Candidate ?
                    "$fwObject = {0}; Get-{1} -Connection $Connection -FromCandidateConfig -FirewallObject $fwObject" :
                    "$fwObject = {0}; Get-{1} -Connection $Connection -FirewallObject $fwObject", objectUnderTest.ToPsScript(), noun);

            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(PsRunner.PipelineContainsFirewallObject<T>(results, objectUnderTest));

            // Clean-up 
            this.ConfigRepository.Delete(objectUnderTest.SchemaName, objectUnderTest.Name);

            return true;
        }

        public bool GetSingleByObjectWhereObjectsAreNotEqualButMatchOnName<T>(string noun, ConfigTypes configType) where T : FirewallObject
        {
            // Setup 
            var objectUnderTest = this.RandomObjectFactory.GenerateRandomObject<T>();
            this.ConfigRepository.Set(objectUnderTest);

            if (configType == ConfigTypes.Running)
            {
                this.CommitCandidateConfig();
            }

            // Test
            objectUnderTest.Mutate();
            var script = string.Format(
                 configType == ConfigTypes.Candidate ?
                     "$fwObject = {0}; Get-{1} -Connection $Connection -FromCandidateConfig -FirewallObject $fwObject" :
                     "$fwObject = {0}; Get-{1} -Connection $Connection -FirewallObject $fwObject", objectUnderTest.ToPsScript(), noun);

            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 0);
            
            // Clean-up 
            this.ConfigRepository.Delete(objectUnderTest.SchemaName, objectUnderTest.Name);

            return true;
        }

        public bool GetMultipleByName<T>(string noun, ConfigTypes configType) where T : FirewallObject
        {
            // Setup 
            // Setup - Ensure that at least 2 addresses are present
            var objectsUnderTest = this.RandomObjectFactory.GenerateRandomObjects<T>();
            foreach (var obj in objectsUnderTest)
            {
                this.ConfigRepository.Set(obj);
            }

            if (configType == ConfigTypes.Running)
            {
                this.CommitCandidateConfig();
            }

            // Test
            var script = string.Format(
                configType == ConfigTypes.Candidate ?
                    "Get-{0} -Connection $Connection -FromCandidateConfig -Name {1},{2}" :
                    "Get-{0} -Connection $Connection -Name {1},{2}", noun, objectsUnderTest[0].Name, objectsUnderTest[1].Name);

            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 2);
            Assert.IsTrue(PsRunner.PipelineContainsFirewallObjects<T>(results, objectsUnderTest));

            // Clean-up 
            foreach (var obj in objectsUnderTest)
            {
                this.ConfigRepository.Delete(obj.SchemaName, obj.Name);
            }
            return true;
        }

        public bool GetMultipleByNameWhereSomeDontExist<T>(string noun, ConfigTypes configType) where T : FirewallObject
        {
            // Setup - Ensure that at least 2 addresses are present
            var objectsUnderTest = this.RandomObjectFactory.GenerateRandomObjects<T>();
            foreach (var obj in objectsUnderTest)
            {
                ConfigRepository.Set(obj);
            }

            if (configType == ConfigTypes.Running)
            {
                CommitCandidateConfig();
            }

            // Test
            var nameBefore = objectsUnderTest[0].Name;
            objectsUnderTest[0].Name += "1";  // this should not exist
            var script = string.Format(
                configType == ConfigTypes.Candidate ?
                    "Get-{0} -Connection $Connection -FromCandidateConfig -Name {1},{2}" :
                    "Get-{0} -Connection $Connection -Name {1},{2}", noun, objectsUnderTest[0].Name, objectsUnderTest[1].Name);

            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(results[0].BaseObject is T);
            Assert.IsTrue(PsRunner.PipelineContainsFirewallObject<T>(results, objectsUnderTest[1]));

            // Clean-up 
            objectsUnderTest[0].Name = nameBefore;
            foreach (var obj in objectsUnderTest)
            {
                this.ConfigRepository.Delete(obj.SchemaName, obj.Name);
            }
            return true;
        }

        public bool GetMultipleByObjectWhereSomeFailEqualsTests<T>(string noun, ConfigTypes configType) where T : FirewallObject
        {
            // Setup - Ensure that at least 2 addresses are present
            var objectsUnderTest = this.RandomObjectFactory.GenerateRandomObjects<T>();
            foreach (var obj in objectsUnderTest)
            {
                this.ConfigRepository.Set(obj);
            }

            if (configType == ConfigTypes.Running)
            {
                this.CommitCandidateConfig();
            }

            // Test
            
            objectsUnderTest[0].Mutate(); // This should not match antything
            var script = string.Format(
                configType == ConfigTypes.Candidate ?
                    "$fwObject1 = {0};$fwObject2 = {1};Get-{2} -Connection $Connection -FromCandidateConfig -FirewallObject $fwObject1, $fwObject2" :
                    "$fwObject1 = {0};$fwObject2 = {1};Get-{2} -Connection $Connection -FirewallObject $fwObject1, $fwObject2",
                        objectsUnderTest[0].ToPsScript(),
                        objectsUnderTest[1].ToPsScript(),
                        noun);

            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(results[0].BaseObject is T);
            Assert.IsTrue(PsRunner.PipelineContainsFirewallObject<T>(results, (T)objectsUnderTest[1]));

            // Clean-up 
            foreach (var obj in objectsUnderTest)
            {
                this.ConfigRepository.Delete(obj.SchemaName, obj.Name);
            }
            return true;
        }

        public bool GetMultipleByObject<T>(string noun, ConfigTypes configType) where T : FirewallObject
        {
            // Setup - Ensure that at least 2 addresses are present
            var objectsUnderTest = this.RandomObjectFactory.GenerateRandomObjects<T>();
            foreach (var obj in objectsUnderTest)
            {
                this.ConfigRepository.Set(obj);
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
                        objectsUnderTest[0].ToPsScript(),
                        objectsUnderTest[1].ToPsScript(),
                        noun);

            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 2);
            Assert.IsTrue(results[0].BaseObject is T);
            Assert.IsTrue(results[1].BaseObject is T);
            Assert.IsTrue(PsRunner.PipelineContainsFirewallObjects<T>(results, objectsUnderTest));

            // Clean-up 
            foreach (var obj in objectsUnderTest)
            {
                this.ConfigRepository.Delete(obj.SchemaName, obj.Name);
            }
            return true;
        }
        
        public bool GetMultipleByObjectFromPipeline<T>(string noun, ConfigTypes configType) where T : FirewallObject
        {
            // Setup - Ensure that at least 2 addresses are present
            var objectsUnderTest = this.RandomObjectFactory.GenerateRandomObjects<T>();
            foreach (var obj in objectsUnderTest)
            {
                this.ConfigRepository.Set(obj);
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
                        objectsUnderTest[0].ToPsScript(),
                        objectsUnderTest[1].ToPsScript(),
                        noun);

            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 2);
            Assert.IsTrue(results[0].BaseObject is T);
            Assert.IsTrue(results[1].BaseObject is T);
            Assert.IsTrue(PsRunner.PipelineContainsFirewallObjects<T>(results, objectsUnderTest));

            // Clean-up 
            foreach (var obj in objectsUnderTest)
            {
                this.ConfigRepository.Delete(obj.SchemaName, obj.Name);
            }

            return true;
        }
        
        public bool GetMultipleByNameFromPipeline<T>(string noun, ConfigTypes configType) where T : FirewallObject
        {
            // Setup - Ensure that at least 2 addresses are present
            var objectsUnderTest = this.RandomObjectFactory.GenerateRandomObjects<T>();
            foreach (var obj in objectsUnderTest)
            {
                this.ConfigRepository.Set(obj);
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
                        objectsUnderTest[0].ToPsScript(),
                        objectsUnderTest[1].ToPsScript(),
                        noun);

            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 2);
            Assert.IsTrue(results[0].BaseObject is T);
            Assert.IsTrue(results[1].BaseObject is T);
            Assert.IsTrue(PsRunner.PipelineContainsFirewallObjects<T>(results, objectsUnderTest));

            // Clean-up 
            foreach (var obj in objectsUnderTest)
            {
                this.ConfigRepository.Delete(obj.SchemaName, obj.Name);
            }

            return true;
        }

        public void RejectInvalidName(string noun)
        {
            // Test
            var script = string.Format("Get-{0} -Connection $Connection -FromCandidateConfig -Name '{1}'", noun, "<script");
            PsRunner.ExecutePanosPowerShellScript(script);
        }
    }
}
