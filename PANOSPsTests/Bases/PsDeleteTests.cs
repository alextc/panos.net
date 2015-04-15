namespace PANOSPsTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PANOS;
    using PANOSLibTest;

    public class PsDeleteTests : BaseConfigTest
    {
        public bool DeleteSingleByObjectPassedAsParameter<TDeserializer, TObject>()
            where TDeserializer : ApiResponse, IPayload
            where TObject : FirewallObject
        {
            // Setup
            var objectUnderTest = this.RandomObjectFactory.GenerateRandomObject<TObject>();
            this.ConfigRepository.Set(objectUnderTest);

            // Test
            var script = string.Format(
                "$obj = {0};Remove-PANOSObject -ConnectionProperties $ConnectionProperties -FirewallObject $obj;",
                    objectUnderTest.ToPsScript());
            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsInstanceOfType(results[0].BaseObject, typeof(string));
            var response = results[0].BaseObject as string;
            Assert.IsNotNull(response);
            Assert.AreEqual(response, objectUnderTest.Name);
            Assert.IsNull(this.ConfigRepository.GetSingle<TDeserializer, TObject>(objectUnderTest.SchemaName, objectUnderTest.Name, ConfigTypes.Candidate));

            return true;
        }

        public bool DeleteMultipleByObjectPassedAsParameter<TDeserializer, TObject>()
            where TDeserializer : ApiResponse, IPayload
            where TObject : FirewallObject
        {
            // Setup
            var objectsUnderTest = this.RandomObjectFactory.GenerateRandomObjects<TObject>(2);
            foreach (var objectUnderTest in objectsUnderTest)
            {
                this.ConfigRepository.Set(objectUnderTest);
            }
            
            // Test
            var script = string.Format(
                "$obj1 = {0}; $obj2 = {1}; Remove-PANOSObject -ConnectionProperties $ConnectionProperties -FirewallObject $obj1, $obj2;",
                    objectsUnderTest[0].ToPsScript(),
                    objectsUnderTest[1].ToPsScript());
            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            for (var i = 0; i < results.Count; i++)
            {
                Assert.IsInstanceOfType(results[i].BaseObject, typeof(string));
                var response = results[i].BaseObject as string;
                Assert.IsNotNull(response);
                Assert.AreEqual(response, objectsUnderTest[i].Name);
                Assert.IsNull(this.ConfigRepository.GetSingle<TDeserializer, TObject>(objectsUnderTest[i].SchemaName, objectsUnderTest[i].Name, ConfigTypes.Candidate));
            }
            
            return true;
        }

        public bool DeleteSingleByNamePassedAsParameter<TDeserializer, TObject>()
            where TDeserializer : ApiResponse, IPayload
            where TObject : FirewallObject
        {
            // Setup
            var objectUnderTest = this.RandomObjectFactory.GenerateRandomObject<TObject>();
            this.ConfigRepository.Set(objectUnderTest);

            // Test
            var script = string.Format(
                "$name = '{0}';Remove-PANOSObject -ConnectionProperties $ConnectionProperties -Name $name -SchemaName {1};",
                    objectUnderTest.Name,
                    objectUnderTest.SchemaName);
            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsInstanceOfType(results[0].BaseObject, typeof(string));
            var response = results[0].BaseObject as string;
            Assert.IsNotNull(response);
            Assert.AreEqual(response, objectUnderTest.Name);
            Assert.IsNull(this.ConfigRepository.GetSingle<TDeserializer, TObject>(objectUnderTest.SchemaName, objectUnderTest.Name, ConfigTypes.Candidate));

            return true;
        }

        public bool DeleteMultipleByNamePassedAsParameter<TDeserializer, TObject>()
            where TDeserializer : ApiResponse, IPayload
            where TObject : FirewallObject
        {
            // Setup
            var objectsUnderTest = this.RandomObjectFactory.GenerateRandomObjects<TObject>(2);
            foreach (var objectUnderTest in objectsUnderTest)
            {
                this.ConfigRepository.Set(objectUnderTest);
            }

            // Test
            var script = string.Format(
                "$name1 = '{0}'; $name2 = '{1}'; Remove-PANOSObject -ConnectionProperties $ConnectionProperties -Name $name1, $name2 -SchemaName {2};",
                    objectsUnderTest[0].Name,
                    objectsUnderTest[1].Name,
                    objectsUnderTest[0].SchemaName);
            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            for (var i = 0; i < results.Count; i++)
            {
                Assert.IsInstanceOfType(results[i].BaseObject, typeof(string));
                var response = results[i].BaseObject as string;
                Assert.IsNotNull(response);
                Assert.AreEqual(response, objectsUnderTest[i].Name);
                Assert.IsNull(this.ConfigRepository.GetSingle<TDeserializer, TObject>(objectsUnderTest[i].SchemaName, objectsUnderTest[i].Name, ConfigTypes.Candidate));
            }

            return true;
        }

        public bool DeleteMultipleByNamePassedViaPipeline<TDeserializer, TObject>()
            where TDeserializer : ApiResponse, IPayload
            where TObject : FirewallObject
        {
            // Setup
            var objectsUnderTest = this.RandomObjectFactory.GenerateRandomObjects<TObject>(2);
            foreach (var objectUnderTest in objectsUnderTest)
            {
                this.ConfigRepository.Set(objectUnderTest);
            }

            // Test
            var script = string.Format(
                "$name1 = '{0}'; $name2 = '{1}'; $name1, $name2 | Remove-PANOSObject -ConnectionProperties $ConnectionProperties -SchemaName {2};",
                    objectsUnderTest[0].Name,
                    objectsUnderTest[1].Name,
                    objectsUnderTest[0].SchemaName);
            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            for (var i = 0; i < results.Count; i++)
            {
                Assert.IsInstanceOfType(results[i].BaseObject, typeof(string));
                var response = results[i].BaseObject as string;
                Assert.IsNotNull(response);
                Assert.AreEqual(response, objectsUnderTest[i].Name);
                Assert.IsNull(this.ConfigRepository.GetSingle<TDeserializer, TObject>(objectsUnderTest[i].SchemaName, objectsUnderTest[i].Name, ConfigTypes.Candidate));
            }

            return true;
        }

        public bool DeleteMultipleByObjectPassedViaPipeline<TDeserializer, TObject>()
            where TDeserializer : ApiResponse, IPayload
            where TObject : FirewallObject
        {
            // Setup
            var objectsUnderTest = this.RandomObjectFactory.GenerateRandomObjects<TObject>(2);
            foreach (var objectUnderTest in objectsUnderTest)
            {
                this.ConfigRepository.Set(objectUnderTest);
            }

            // Test
            var script = string.Format(
                "$obj1 = {0}; $obj2 = {1}; $obj1, $obj2 | Remove-PANOSObject -ConnectionProperties $ConnectionProperties -SchemaName {2};",
                    objectsUnderTest[0].ToPsScript(),
                    objectsUnderTest[1].ToPsScript(),
                    objectsUnderTest[0].SchemaName);
            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            for (var i = 0; i < results.Count; i++)
            {
                Assert.IsInstanceOfType(results[i].BaseObject, typeof(string));
                var response = results[i].BaseObject as string;
                Assert.IsNotNull(response);
                Assert.AreEqual(response, objectsUnderTest[i].Name);
                Assert.IsNull(this.ConfigRepository.GetSingle<TDeserializer, TObject>(objectsUnderTest[i].SchemaName, objectsUnderTest[i].Name, ConfigTypes.Candidate));
            }

            return true;
        }

    }
}
