namespace PANOSPsTest
{
    using NUnit.Framework;
    using PANOS;

    [TestFixture]
    public class CommitUnitTests : BasePsTest
    {
        [Test]
        public void CommitTest()
        {
            // Setup
            var newObj = this.RandomObjectFactory.GenerateRandomObject<AddressObject>();
            this.ConfigRepository.Set(newObj);
            var makeChangeScript = string.Format(
                "$obj = {0};Add-PANOSObject -ConnectionProperties $ConnectionProperties -FirewallObject $obj", newObj.ToPsScript());
            PsRunner.ExecutePanosPowerShellScript(makeChangeScript);

            // Test
            const string CommitScript = "Save-PANOSChanges -ConnectionProperties $ConnectionProperties";
            var results = PsRunner.ExecutePanosPowerShellScript(CommitScript);

            // Validate
            Assert.IsNotNull(results[0]);
            var apiEnqueuedResponse = results[0].BaseObject as ApiEnqueuedResponse;
            Assert.IsNotNull(apiEnqueuedResponse);
            var job = apiEnqueuedResponse.Job;
            Assert.IsNotNull(job);
            Assert.IsTrue(job.Id > 0);
            
            // Cleanup
            this.ConfigRepository.Delete(newObj.SchemaName, newObj.Name);    
        }

        [Test]
        [ExpectedException(typeof(System.Management.Automation.CmdletInvocationException))]
        public void CommitWhileAnotherCommitIsInProgressTest()
        {
            // Setup
            var newObj = this.RandomObjectFactory.GenerateRandomObject<AddressObject>();
            this.ConfigRepository.Set(newObj);
            var makeChangeScript = string.Format(
                "$obj = {0};Add-PANOSObject -ConnectionProperties $ConnectionProperties -FirewallObject $obj", newObj.ToPsScript());
            PsRunner.ExecutePanosPowerShellScript(makeChangeScript);

            // Test
            const string CommitScript = "Save-PANOSChanges -ConnectionProperties $ConnectionProperties";
            PsRunner.ExecutePanosPowerShellScript(CommitScript);
            PsRunner.ExecutePanosPowerShellScript(CommitScript);
        }
    }
}
