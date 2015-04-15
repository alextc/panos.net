namespace PANOSLibTest
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PANOS;

    public class GetTests : BaseConfigTest
    {
        public bool GetAllObjects<TDeserializer, TObject>(string schemaName, ConfigTypes configType) 
            where TDeserializer : ApiResponse, IDictionaryPayload where TObject : FirewallObject
        {
            // Setup - Ensure that at least 2 addresses are present
            var objectsUnderTest = new List<TObject>
                {
                    RandomObjectFactory.GenerateRandomObject<TObject>(),
                    RandomObjectFactory.GenerateRandomObject<TObject>()
                };
            Assert.IsNotNull(this.ConfigRepository.Set(objectsUnderTest[0]));
            Assert.IsNotNull(this.ConfigRepository.Set(objectsUnderTest[1]));

            if (configType == ConfigTypes.Running)
            {
                CommitCandidateConfig();
            }

            // Test
            var result = this.ConfigRepository.GetAll<TDeserializer, TObject>(schemaName, configType);
            foreach (var obj in objectsUnderTest)
            {
                var match = result[obj.Name];
                Assert.IsNotNull(match);
                Assert.IsInstanceOfType(match, typeof(TObject));
                Assert.AreEqual(match, obj);
            }

            // Clean-up
            foreach (var obj in objectsUnderTest)
            {
                Assert.IsNotNull(this.ConfigRepository.Delete(schemaName, obj.Name));

                // If this is a group object, delete its members
                // TODO: Deal with nested Groups
                if (obj is AddressGroupObject)
                {
                    foreach (var member in (obj as AddressGroupObject).Members)
                    {
                        Assert.IsNotNull(this.ConfigRepository.Delete(Schema.AddressSchemaName, member));
                    }
                }
            }

            return true;
        }

        public bool GetSingleObject<TDeserializer, TObject>(string schemaName, ConfigTypes configType)
            where TDeserializer : ApiResponse, IPayload
            where TObject : FirewallObject
        {
            // Setup
            var objectUnderTest = RandomObjectFactory.GenerateRandomObject<TObject>();
            Assert.IsNotNull(this.ConfigRepository.Set(objectUnderTest));

            if (configType == ConfigTypes.Running)
            {
                CommitCandidateConfig();
            }
            
            // Test
            Assert.IsNotNull(this.ConfigRepository.GetSingle<TDeserializer, TObject>(schemaName, objectUnderTest.Name, configType));

            // Clean-up
            Assert.IsNotNull(this.ConfigRepository.Delete(schemaName, objectUnderTest.Name));

            return true;
        }

        public bool GetNonExistingObject<TDeserializer, TObject>(string schemaName, ConfigTypes configType)
            where TDeserializer : ApiResponse, IPayload
            where TObject : FirewallObject
        {
            var objectUnderTest = RandomObjectFactory.GenerateRandomObject<TObject>();
            Assert.IsNull(this.ConfigRepository.GetSingle<TDeserializer, TObject>(schemaName, objectUnderTest.Name, configType));
            return true;
        }
    }
}
