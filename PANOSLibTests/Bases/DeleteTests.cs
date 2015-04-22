namespace PANOSLibTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PANOS;

    public class DeleteTests : BaseConfigTest
    {

        public bool DeleteObject<TDeserializer, TObject>()
            where TObject : FirewallObject
            where TDeserializer : ApiResponse, IPayload
        {
            // Precondition
            var objUnderTest = RandomObjectFactory.GenerateRandomObject<TObject>();
            ConfigRepository.Set(objUnderTest);

            // Test
            Assert.IsNotNull(this.ConfigRepository.Delete(objUnderTest.SchemaName, objUnderTest.Name));

            // Postcondition
            Assert.IsNull(this.ConfigRepository.GetSingle<TDeserializer, TObject>(objUnderTest.SchemaName, objUnderTest.Name, ConfigTypes.Candidate));

            return true;
        }


        public void DeleteNonExistingObject<TObject>() where TObject : FirewallObject
        {
            // Precondition
            var obj = RandomObjectFactory.GenerateRandomObject<TObject>();

            // Test
            this.ConfigRepository.Delete(obj.SchemaName, obj.Name);
        }
    }
}
