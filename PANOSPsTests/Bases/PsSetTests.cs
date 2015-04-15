namespace PANOSPsTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PANOS;
    using PANOSLibTest;

    public class PsSetTests : BaseConfigTest
    {
        // TODO: Test to simulate a failure from PANOS during addition.
        // Not sure how to inject a fault at the moment, perhaps illegal character in the name?

        public bool SetSingleFromObjectPassedAsParameter<TDeserializer, TObject>() where TDeserializer : ApiResponse, IPayload  where TObject : FirewallObject
        {
            // Setup
            var newObj = this.RandomObjectFactory.GenerateRandomObject<TObject>();
            this.ConfigRepository.Set(newObj);

            // Test
            var script = string.Format(
                "$obj = {0};Add-PANOSObject -ConnectionProperties $ConnectionProperties -FirewallObject $obj", newObj.ToPsScript());
            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsNotNull(results[0]);
            var apiResponseWithMessage = results[0].BaseObject as ApiResponseWithMessage;
            Assert.IsNotNull(apiResponseWithMessage);
            Assert.AreEqual(apiResponseWithMessage.ObjectActedUpon, newObj);

            var confirmationObject = this.ConfigRepository.GetSingle<TDeserializer, TObject>(newObj.SchemaName, newObj.Name, ConfigTypes.Candidate);
            Assert.AreEqual(newObj, confirmationObject);

            // Cleanup
            this.ConfigRepository.Delete(newObj.SchemaName, newObj.Name);

            return true;
        }

        public bool SetMultipleFromObjectPassedAsParameter<TDeserializer, TObject>()
            where TDeserializer : ApiResponse, IPayload
            where TObject : FirewallObject
        {
            // Setup
            var newObjs = this.RandomObjectFactory.GenerateRandomObjects<TObject>(2);
            foreach (var obj in newObjs)
            {
                this.ConfigRepository.Set(obj);
            }
            
            // Test
            var script = string.Format(
                "$obj1 = {0};$obj2={1};Add-PANOSObject -ConnectionProperties $ConnectionProperties -FirewallObject $obj1,$obj2",
                    newObjs[0].ToPsScript(),
                    newObjs[1].ToPsScript());
            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            for(var i=0; i< results.Count; i++)
            {
                Assert.IsNotNull(results[i]);
                var apiResponseWithMessage = results[i].BaseObject as ApiResponseWithMessage;
                Assert.IsNotNull(apiResponseWithMessage);
                Assert.AreEqual(apiResponseWithMessage.ObjectActedUpon, newObjs[i]);

                var confirmationObject = this.ConfigRepository.GetSingle<TDeserializer, TObject>(newObjs[i].SchemaName, newObjs[i].Name, ConfigTypes.Candidate);
                Assert.AreEqual(newObjs[i], confirmationObject);
            }
            
            // Cleanup
            foreach (var obj in newObjs)
            {
                this.ConfigRepository.Delete(obj.SchemaName, obj.Name);
            }
            
            return true;
        }

        public bool SetMultipleFromObjectPassedFromPipeline<TDeserializer, TObject>()
            where TDeserializer : ApiResponse, IPayload
            where TObject : FirewallObject
        {
            // Setup
            var newObjs = this.RandomObjectFactory.GenerateRandomObjects<TObject>(2);
            foreach (var obj in newObjs)
            {
                this.ConfigRepository.Set(obj);
            }

            // Test
            var script = string.Format(
                "$obj1 = {0};$obj2={1}; $obj1,$obj2 | Add-PANOSObject -ConnectionProperties $ConnectionProperties",
                    newObjs[0].ToPsScript(),
                    newObjs[1].ToPsScript());
            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            for (var i = 0; i < results.Count; i++)
            {
                Assert.IsNotNull(results[i]);
                var apiResponseWithMessage = results[i].BaseObject as ApiResponseWithMessage;
                Assert.IsNotNull(apiResponseWithMessage);
                Assert.AreEqual(apiResponseWithMessage.ObjectActedUpon, newObjs[i]);

                var confirmationObject = this.ConfigRepository.GetSingle<TDeserializer, TObject>(newObjs[i].SchemaName, newObjs[i].Name, ConfigTypes.Candidate);
                Assert.AreEqual(newObjs[i], confirmationObject);
            }

            // Cleanup
            foreach (var obj in newObjs)
            {
                this.ConfigRepository.Delete(obj.SchemaName, obj.Name);
            }

            return true;
        }
    }
}
