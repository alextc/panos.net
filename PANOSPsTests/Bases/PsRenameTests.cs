namespace PANOSPsTest
{
    using NUnit.Framework;
    using PANOS;
    
    public class PsRenameTests : BasePsTest
    {
        public bool RenameSingleFromObjectPassedAsParameter<TDeserializer, TObject>()
            where TDeserializer : ApiResponseForGetSingle
            where TObject : FirewallObject
        {
            // Setup
            var objectUnderTest = this.RandomObjectFactory.GenerateRandomObject<TObject>();
            this.ConfigRepository.Set(objectUnderTest);
            var newName = RandomObjectFactory.GenerateRandomName();
            Assert.AreNotEqual(objectUnderTest.Name, newName);
            Assert.IsNull(this.ConfigRepository.GetSingle<TDeserializer, TObject>(objectUnderTest.SchemaName, newName, ConfigTypes.Candidate));

            // Test
            var script = string.Format(
                "$obj = {0};$newName = '{1}';Rename-PANOSObject -ConnectionProperties $ConnectionProperties -FirewallObject $obj -NewName $newName;",
                    objectUnderTest.ToPsScript(),
                    newName);
            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.That(results[0].BaseObject, Is.InstanceOf<string>());
            var response = results[0].BaseObject as string;
            Assert.IsNotNull(response);
            Assert.AreEqual(response, newName);
            Assert.IsNotNull(this.ConfigRepository.GetSingle<TDeserializer, TObject>(objectUnderTest.SchemaName, newName, ConfigTypes.Candidate));
            Assert.IsNull(this.ConfigRepository.GetSingle<TDeserializer, TObject>(objectUnderTest.SchemaName, objectUnderTest.Name, ConfigTypes.Candidate));

            // Cleanup
            this.ConfigRepository.Delete(objectUnderTest.SchemaName, newName);

            return true;
        }

        public bool RenameSingleFromNamePassedAsParameter<TDeserializer, TObject>()
            where TDeserializer : ApiResponseForGetSingle
            where TObject : FirewallObject
        {
            // Setup
            var objectUnderTest = this.RandomObjectFactory.GenerateRandomObject<TObject>();
            this.ConfigRepository.Set(objectUnderTest);
            var newName = RandomObjectFactory.GenerateRandomName();
            Assert.AreNotEqual(objectUnderTest.Name, newName);
            Assert.IsNull(this.ConfigRepository.GetSingle<TDeserializer, TObject>(objectUnderTest.SchemaName, newName, ConfigTypes.Candidate));

            // Test
            var script = string.Format(
                "$name = '{0}';$newName = '{1}';Rename-PANOSObject -ConnectionProperties $ConnectionProperties -Name $name -NewName $newName -SchemaName {2};",
                    objectUnderTest.Name,
                    newName,
                    objectUnderTest.SchemaName);
            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.That(results[0].BaseObject, Is.InstanceOf<string>());
            var response = results[0].BaseObject as string;
            Assert.IsNotNull(response);
            Assert.AreEqual(response, newName);
            Assert.IsNotNull(this.ConfigRepository.GetSingle<TDeserializer, TObject>(objectUnderTest.SchemaName, newName, ConfigTypes.Candidate));
            Assert.IsNull(this.ConfigRepository.GetSingle<TDeserializer, TObject>(objectUnderTest.SchemaName, objectUnderTest.Name, ConfigTypes.Candidate));

            // Cleanup
            this.ConfigRepository.Delete(objectUnderTest.SchemaName, newName);

            return true;
        }

        // This unit tests is based on a scenario where the object is passed via Pipeline and the new name is generated based on the existing name (adding 00 in this case).
        public bool RenameMultipleFromObjectsPassedViaPipeline<TDeserializer, TObject>()
            where TDeserializer : ApiResponseForGetSingle
            where TObject : FirewallObject
        {
            // Setup
            var objectsUnderTest = this.RandomObjectFactory.GenerateRandomObjects<TObject>(2);
            foreach (var obj in objectsUnderTest)
            {
                this.ConfigRepository.Set(obj);
            }
            
            // Test
            var script = string.Format(
                "$obj1 = {0};$obj2 = {1};$obj1, $obj2 | Rename-PANOSObject -ConnectionProperties $ConnectionProperties -NewName {{$_.Name + '00'}}",
                    objectsUnderTest[0].ToPsScript(),
                    objectsUnderTest[1].ToPsScript());

            var results = PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsTrue(results.Count == 2);

            for (var i = 0; i < results.Count; i++)
            {
                Assert.That(results[i].BaseObject, Is.InstanceOf<string>());
                var response = results[i].BaseObject as string;
                Assert.IsNotNull(response);
                Assert.AreEqual(response, objectsUnderTest[i].Name + "00");
                Assert.IsNotNull(this.ConfigRepository.GetSingle<TDeserializer, TObject>(objectsUnderTest[i].SchemaName, objectsUnderTest[i].Name + "00", ConfigTypes.Candidate));
                Assert.IsNull(this.ConfigRepository.GetSingle<TDeserializer, TObject>(objectsUnderTest[i].SchemaName, objectsUnderTest[i].Name, ConfigTypes.Candidate));
            }
            
            // Clenup
            foreach (var obj in objectsUnderTest)
            {
                this.ConfigRepository.Delete(obj.SchemaName, obj.Name + "00");
            }

            return true;
        }
    }
}
