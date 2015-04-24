namespace PANOSLibTest
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PANOS;

    public class DeleteTests : BaseConfigTest
    {
        public void DeleteObject<TDeserializer, TObject>()
            where TObject : FirewallObject
            where TDeserializer : ApiResponse, IPayload
        {
            // Precondition
            var objUnderTest = RandomObjectFactory.GenerateRandomObject<TObject>();
            ConfigRepository.Set(objUnderTest);

            // Test
            ConfigRepository.Delete(objUnderTest.SchemaName, objUnderTest.Name);

            // Postcondition
            Assert.IsFalse(ConfigRepository.GetSingle<TDeserializer, TObject>(objUnderTest.SchemaName, objUnderTest.Name, ConfigTypes.Candidate).Any());
        }


        public void DeleteNonExistingObject<TObject>() where TObject : FirewallObject
        {
            // Precondition
            var obj = RandomObjectFactory.GenerateRandomObject<TObject>();

            // Test
            ConfigRepository.Delete(obj.SchemaName, obj.Name);
        }
    }
}
