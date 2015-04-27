namespace PANOSPsTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PANOS;
    using PANOSLibTest;

    public class PsAddTests : BaseConfigTest
    {
        // TODO: Test to simulate a failure from PANOS during addition.
        // Not sure how to inject a fault at the moment, perhaps illegal character in the name?

        public void SetSingleFromObjectPassedAsParameter<TDeserializer, TObject>(string verb) 
            where TDeserializer : ApiResponseForGetSingle  where TObject : FirewallObject
        {
            // Setup
            var newObj = RandomObjectFactory.GenerateRandomObject<TObject>();
            ConfigRepository.Set(newObj);

            // Test
            var script = string.Format(
                "$obj = {0};Add-{1} -Connection $Connection -{2} $obj", newObj.ToPsScript(), verb, verb);
            PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            var confirmationObject = ConfigRepository.GetSingle<TDeserializer, TObject>(newObj.SchemaName, newObj.Name, ConfigTypes.Candidate);
            Assert.AreEqual(newObj, confirmationObject);

            // Cleanup
            ConfigRepository.Delete(newObj.SchemaName, newObj.Name);
        }

        public void PassThruTest<TDeserializer, TObject>(string verb)
            where TDeserializer : ApiResponseForGetSingle
            where TObject : FirewallObject
        {
            // Setup
            var newObj = RandomObjectFactory.GenerateRandomObject<TObject>();
            ConfigRepository.Set(newObj);

            // Test
            var script = string.Format(
                "$obj = {0};Add-{1} -Connection $Connection -{2} $obj -PassThru", newObj.ToPsScript(), verb, verb);
            var pipeline =  PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsNotNull(pipeline[0]);
            var passThruObject = pipeline[0].BaseObject as TObject;
            Assert.IsNotNull(passThruObject);
            Assert.AreEqual(passThruObject, newObj);
            var confirmationObject = ConfigRepository.GetSingle<TDeserializer, TObject>(newObj.SchemaName, newObj.Name, ConfigTypes.Candidate);
            Assert.AreEqual(newObj, confirmationObject);

            // Cleanup
            ConfigRepository.Delete(newObj.SchemaName, newObj.Name);
        }

        public void SetMultipleFromObjectPassedAsParameter<TDeserializer, TObject>(string verb)
            where TDeserializer : ApiResponseForGetSingle
            where TObject : FirewallObject
        {
            // Setup
            var newObjs = RandomObjectFactory.GenerateRandomObjects<TObject>();
            foreach (var obj in newObjs)
            {
                this.ConfigRepository.Set(obj);
            }
            
            // Test
            var script = string.Format(
                "$obj1 = {0};$obj2={1};Add-{2} -Connection $Connection -{3} $obj1,$obj2",
                    newObjs[0].ToPsScript(),
                    newObjs[1].ToPsScript(),
                    verb, 
                    verb);
            PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            foreach (var obj in newObjs)
            {
                var confirmationObject = ConfigRepository.GetSingle<TDeserializer, TObject>(obj.SchemaName, obj.Name, ConfigTypes.Candidate);
                Assert.AreEqual(obj, confirmationObject);
            }
            
            // Cleanup
            foreach (var obj in newObjs)
            {
                ConfigRepository.Delete(obj.SchemaName, obj.Name);
            }
        }

        public void SetMultipleFromObjectPassedFromPipeline<TDeserializer, TObject>(string verb)
            where TDeserializer : ApiResponseForGetSingle
            where TObject : FirewallObject
        {
            // Setup
            var newObjs = RandomObjectFactory.GenerateRandomObjects<TObject>();
            foreach (var obj in newObjs)
            {
                ConfigRepository.Set(obj);
            }

            // Test
            var script = string.Format(
                "$obj1 = {0};$obj2={1}; $obj1,$obj2 | Add-{2} -Connection $Connection",
                    newObjs[0].ToPsScript(),
                    newObjs[1].ToPsScript(),
                    verb);
            PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            foreach (var obj in newObjs)
            {
                var confirmationObject = this.ConfigRepository.GetSingle<TDeserializer, TObject>(obj.SchemaName, obj.Name, ConfigTypes.Candidate);
                Assert.AreEqual(obj, confirmationObject);
            }
            
            // Cleanup
            foreach (var obj in newObjs)
            {
                ConfigRepository.Delete(obj.SchemaName, obj.Name);
            }
        }
    }
}
