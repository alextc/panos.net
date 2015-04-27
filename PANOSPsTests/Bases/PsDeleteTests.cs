namespace PANOSPsTest
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PANOS;
    using PANOSLibTest;

    public class PsDeleteTests : BaseConfigTest
    {
        public void DeleteSingleByObjectPassedAsParameter<TDeserializer, TObject>(string noun)
            where TDeserializer : ApiResponseForGetSingle
            where TObject : FirewallObject
        {
            // Setup
            var objectUnderTest = this.RandomObjectFactory.GenerateRandomObject<TObject>();
            this.ConfigRepository.Set(objectUnderTest);

            // Test
            var script = string.Format(
                "$obj = {0};Remove-{1} -Connection $Connection -{2} $obj;",
                    objectUnderTest.ToPsScript(),
                    noun,
                    noun);
            PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsFalse(
                ConfigRepository.
                    GetSingle<TDeserializer, TObject>(objectUnderTest.SchemaName, objectUnderTest.Name, ConfigTypes.Candidate).
                    Any());
            
        }

        public void DeleteMultipleByObjectPassedAsParameter<TDeserializer, TObject>(string noun)
            where TDeserializer : ApiResponseForGetSingle
            where TObject : FirewallObject
        {
            // Setup
            var objectsUnderTest = this.RandomObjectFactory.GenerateRandomObjects<TObject>();
            foreach (var objectUnderTest in objectsUnderTest)
            {
                this.ConfigRepository.Set(objectUnderTest);
            }
            
            // Test
            var script = string.Format(
                "$obj1 = {0}; $obj2 = {1}; Remove-{2} -Connection $Connection -{3} $obj1, $obj2;",
                    objectsUnderTest[0].ToPsScript(),
                    objectsUnderTest[1].ToPsScript(),
                    noun,
                    noun);
            PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            foreach (var objectUnderTest in objectsUnderTest)
            {
                Assert.IsFalse(
                ConfigRepository.
                    GetSingle<TDeserializer, TObject>(objectUnderTest.SchemaName, objectUnderTest.Name, ConfigTypes.Candidate).
                    Any());
            }
        }

        public void DeleteSingleByNamePassedAsParameter<TDeserializer, TObject>(string noun)
            where TDeserializer : ApiResponseForGetSingle
            where TObject : FirewallObject
        {
            // Setup
            var objectUnderTest = RandomObjectFactory.GenerateRandomObject<TObject>();
            this.ConfigRepository.Set(objectUnderTest);

            // Test
            var script = string.Format(
                "$name = '{0}';Remove-{1} -Connection $Connection -Name $name;",
                    objectUnderTest.Name,
                    noun);
            PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsFalse(
                ConfigRepository.
                    GetSingle<TDeserializer, TObject>(objectUnderTest.SchemaName, objectUnderTest.Name, ConfigTypes.Candidate).
                    Any());
        }

        public void DeleteMultipleByNamePassedAsParameter<TDeserializer, TObject>(string noun)
            where TDeserializer : ApiResponseForGetSingle
            where TObject : FirewallObject
        {
            // Setup
            var objectsUnderTest = this.RandomObjectFactory.GenerateRandomObjects<TObject>();
            foreach (var objectUnderTest in objectsUnderTest)
            {
                this.ConfigRepository.Set(objectUnderTest);
            }

            // Test
            var script = string.Format(
                "$name1 = '{0}'; $name2 = '{1}'; Remove-{2} -Connection $Connection -Name $name1, $name2;",
                    objectsUnderTest[0].Name,
                    objectsUnderTest[1].Name,
                    noun);
            PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            foreach (var objectUnderTest in objectsUnderTest)
            {
                Assert.IsFalse(
                ConfigRepository.
                    GetSingle<TDeserializer, TObject>(objectUnderTest.SchemaName, objectUnderTest.Name, ConfigTypes.Candidate).
                    Any());
            }
        }

        public void DeleteMultipleByNamePassedViaPipeline<TDeserializer, TObject>(string noun)
            where TDeserializer : ApiResponseForGetSingle
            where TObject : FirewallObject
        {
            // Setup
            var objectsUnderTest = RandomObjectFactory.GenerateRandomObjects<TObject>();
            foreach (var objectUnderTest in objectsUnderTest)
            {
                ConfigRepository.Set(objectUnderTest);
            }

            // Test
            var script = string.Format(
                "$name1 = '{0}'; $name2 = '{1}'; $name1, $name2 | Remove-{2} -Connection $Connection;",
                    objectsUnderTest[0].Name,
                    objectsUnderTest[1].Name,
                    noun);
            PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            foreach (var objectUnderTest in objectsUnderTest)
            {
                Assert.IsFalse(
                ConfigRepository.
                    GetSingle<TDeserializer, TObject>(objectUnderTest.SchemaName, objectUnderTest.Name, ConfigTypes.Candidate).
                    Any());
            }
        }

        public void DeleteMultipleByObjectPassedViaPipeline<TDeserializer, TObject>(string noun)
            where TDeserializer : ApiResponseForGetSingle where TObject : FirewallObject
        {
            // Setup
            var objectsUnderTest = this.RandomObjectFactory.GenerateRandomObjects<TObject>();
            foreach (var objectUnderTest in objectsUnderTest)
            {
                this.ConfigRepository.Set(objectUnderTest);
            }

            // Test
            var script =
                string.Format(
                    "$obj1 = {0}; $obj2 = {1}; $obj1, $obj2 | Remove-{2} -Connection $Connection;",
                    objectsUnderTest[0].ToPsScript(),
                    objectsUnderTest[1].ToPsScript(),
                    noun);
            PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            foreach (var objectUnderTest in objectsUnderTest)
            {
                Assert.IsFalse(
                ConfigRepository.
                    GetSingle<TDeserializer, TObject>(objectUnderTest.SchemaName, objectUnderTest.Name, ConfigTypes.Candidate).
                    Any());
            }
        }

        public void DeleteAndPassThruObjectTest<TDeserializer, TObject>(string noun)
            where TDeserializer : ApiResponseForGetSingle
            where TObject : FirewallObject
        {

            // Setup
            var objectUnderTest = this.RandomObjectFactory.GenerateRandomObject<TObject>();
            this.ConfigRepository.Set(objectUnderTest);

            // Test
            var script = string.Format(
                "$obj = {0};Remove-{1} -Connection $Connection -{2} $obj -PassThru;",
                    objectUnderTest.ToPsScript(),
                    noun,
                    noun);
            var pipeline =  PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsFalse(
                ConfigRepository.
                    GetSingle<TDeserializer, TObject>(objectUnderTest.SchemaName, objectUnderTest.Name, ConfigTypes.Candidate).
                    Any());

            Assert.IsNotNull(pipeline[0]);
            var passThruObject = pipeline[0].BaseObject as TObject;
            Assert.IsNotNull(passThruObject);
            Assert.AreEqual(passThruObject, objectUnderTest);
        }

        public void DeleteAndPassThruName<TDeserializer, TObject>(string noun)
            where TDeserializer : ApiResponseForGetSingle
            where TObject : FirewallObject
        {
            // Setup
            var objectUnderTest = RandomObjectFactory.GenerateRandomObject<TObject>();
            this.ConfigRepository.Set(objectUnderTest);

            // Test
            var script = string.Format(
                "$name = '{0}';Remove-{1} -Connection $Connection -Name $name -PassThru;",
                    objectUnderTest.Name,
                    noun);
            var pipeline =  PsRunner.ExecutePanosPowerShellScript(script);

            // Validate
            Assert.IsFalse(
                ConfigRepository.
                    GetSingle<TDeserializer, TObject>(objectUnderTest.SchemaName, objectUnderTest.Name, ConfigTypes.Candidate).
                    Any());

            Assert.IsNotNull(pipeline[0]);
            var passThruName = pipeline[0].BaseObject as String;
            Assert.IsNotNull(passThruName);
            Assert.AreEqual(passThruName, objectUnderTest.Name);
        }
    }
}
