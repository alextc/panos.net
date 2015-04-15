﻿namespace PANOSPsTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using PANOS;

    using PANOSLibTest;

    public class PsGetTests : BaseConfigTest
    {
        public bool GetAll<T>(string getVerb, ConfigTypes configType) where T: FirewallObject
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
                    "Get-{0} -ConnectionProperties $ConnectionProperties -FromCandidateConfig" :
                    "Get-{0} -ConnectionProperties $ConnectionProperties", getVerb);

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

        public bool GetSingleByName<T>(string getVerb, ConfigTypes configType) where T : FirewallObject
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
                    "Get-{0} -ConnectionProperties $ConnectionProperties -FromCandidateConfig -Name {1}" :
                    "Get-{0} -ConnectionProperties $ConnectionProperties -Name {1}", getVerb, objectUnderTest.Name );

            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(PsRunner.PipelineContainsFirewallObject<T>(results, objectUnderTest));

            // Clean-up 
            this.ConfigRepository.Delete(objectUnderTest.SchemaName, objectUnderTest.Name);
            
            return true;
        }

        public bool GetSingleByObject<T>(string getVerb, ConfigTypes configType) where T : FirewallObject
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
                    "$fwObject = {0}; Get-{1} -ConnectionProperties $ConnectionProperties -FromCandidateConfig -FirewallObject $fwObject" :
                    "$fwObject = {0}; Get-{1} -ConnectionProperties $ConnectionProperties -FirewallObject $fwObject", objectUnderTest.ToPsScript(), getVerb);

            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(PsRunner.PipelineContainsFirewallObject<T>(results, objectUnderTest));

            // Clean-up 
            this.ConfigRepository.Delete(objectUnderTest.SchemaName, objectUnderTest.Name);

            return true;
        }

        public bool GetSingleByObjectWhereObjectsAreNotEqualButMatchOnName<T>(string getVerb, ConfigTypes configType) where T : FirewallObject
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
                     "$fwObject = {0}; Get-{1} -ConnectionProperties $ConnectionProperties -FromCandidateConfig -FirewallObject $fwObject" :
                     "$fwObject = {0}; Get-{1} -ConnectionProperties $ConnectionProperties -FirewallObject $fwObject", objectUnderTest.ToPsScript(), getVerb);

            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(results[0].BaseObject is ObjectNotFoundError);
            
            // Clean-up 
            this.ConfigRepository.Delete(objectUnderTest.SchemaName, objectUnderTest.Name);

            return true;
        }

        public bool GetMultipleByName<T>(string getVerb, ConfigTypes configType) where T : FirewallObject
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
                    "Get-{0} -ConnectionProperties $ConnectionProperties -FromCandidateConfig -Name {1},{2}" :
                    "Get-{0} -ConnectionProperties $ConnectionProperties -Name {1},{2}", getVerb, objectsUnderTest[0].Name, objectsUnderTest[1].Name);

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

        public bool GetMultipleByNameWhereSomeDontExist<T>(string getVerb, ConfigTypes configType) where T : FirewallObject
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
            var nameBefore = objectsUnderTest[0].Name;
            objectsUnderTest[0].Name += "1";
            var script = string.Format(
                configType == ConfigTypes.Candidate ?
                    "Get-{0} -ConnectionProperties $ConnectionProperties -FromCandidateConfig -Name {1},{2}" :
                    "Get-{0} -ConnectionProperties $ConnectionProperties -Name {1},{2}", getVerb, objectsUnderTest[0].Name, objectsUnderTest[1].Name);

            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 2);
            Assert.IsTrue(results[0].BaseObject is ObjectNotFoundError);
            Assert.IsTrue(results[1].BaseObject is T);
            Assert.IsTrue(PsRunner.PipelineContainsFirewallObject<T>(results, objectsUnderTest[1]));

            // Clean-up 
            objectsUnderTest[0].Name = nameBefore;
            foreach (var obj in objectsUnderTest)
            {
                this.ConfigRepository.Delete(obj.SchemaName, obj.Name);
            }
            return true;
        }

        public bool GetMultipleByObjectWhereSomeFailEqualsTests<T>(string getVerb, ConfigTypes configType) where T : FirewallObject
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
            
            objectsUnderTest[0].Mutate();
            var script = string.Format(
                configType == ConfigTypes.Candidate ?
                    "$fwObject1 = {0};$fwObject2 = {1};Get-{2} -ConnectionProperties $ConnectionProperties -FromCandidateConfig -FirewallObject $fwObject1, $fwObject2" :
                    "$fwObject1 = {0};$fwObject2 = {1};Get-{2} -ConnectionProperties $ConnectionProperties -FirewallObject $fwObject1, $fwObject2",
                        objectsUnderTest[0].ToPsScript(),
                        objectsUnderTest[1].ToPsScript(),
                        getVerb);

            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 2);
            Assert.IsTrue(results[0].BaseObject is ObjectNotFoundError);
            Assert.IsTrue(results[1].BaseObject is T);
            Assert.IsTrue(PsRunner.PipelineContainsFirewallObject<T>(results, (T)objectsUnderTest[1]));

            // Clean-up 
            foreach (var obj in objectsUnderTest)
            {
                this.ConfigRepository.Delete(obj.SchemaName, obj.Name);
            }
            return true;
        }

        public bool GetMultipleByObject<T>(string getVerb, ConfigTypes configType) where T : FirewallObject
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
                    "$fwObject1 = {0};$fwObject2 = {1};Get-{2} -ConnectionProperties $ConnectionProperties -FromCandidateConfig -FirewallObject $fwObject1, $fwObject2" :
                    "$fwObject1 = {0};$fwObject2 = {1};Get-{2} -ConnectionProperties $ConnectionProperties -FirewallObject $fwObject1, $fwObject2",
                        objectsUnderTest[0].ToPsScript(),
                        objectsUnderTest[1].ToPsScript(),
                        getVerb);

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
        
        public bool GetMultipleByObjectFromPipeline<T>(string getVerb, ConfigTypes configType) where T : FirewallObject
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
                    "$fwObject1 = {0};$fwObject2 = {1};$fwObject1,$fwObject2 | Get-{2} -ConnectionProperties $ConnectionProperties -FromCandidateConfig" :
                    "$fwObject1 = {0};$fwObject2 = {1};$fwObject1,$fwObject2 | Get-{2} -ConnectionProperties $ConnectionProperties",
                        objectsUnderTest[0].ToPsScript(),
                        objectsUnderTest[1].ToPsScript(),
                        getVerb);

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

        public bool GetMultipleByNameFromPipeline<T>(string getVerb, ConfigTypes configType) where T : FirewallObject
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
                    "$fwObject1 = {0};$fwObject2 = {1};$fwObject1.Name,$fwObject2.Name | Get-{2} -ConnectionProperties $ConnectionProperties -FromCandidateConfig" :
                    "$fwObject1 = {0};$fwObject2 = {1};$fwObject1.Name,$fwObject2.Name | Get-{2} -ConnectionProperties $ConnectionProperties",
                        objectsUnderTest[0].ToPsScript(),
                        objectsUnderTest[1].ToPsScript(),
                        getVerb);

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

        // Must throw Exception
        public void RejectInvalidName(string getVerb)
        {
            // Test
            var script = string.Format("Get-{0} -ConnectionProperties $ConnectionProperties -FromCandidateConfig -Name '{1}'", getVerb, "<script");
            PsRunner.ExecutePanosPowerShellScript(script);
        }
    }
}