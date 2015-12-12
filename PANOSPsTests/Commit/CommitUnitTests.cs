namespace PANOSPsTest
{
    using System.Management.Automation;
    using NUnit.Framework;
    using PANOS;

    [TestFixture]
    public class CommitUnitTests : BasePsTest
    {
        [Test]
        public void CommitTest()
        {
            // Setup
            var newObj = RandomObjectFactory.GenerateRandomObject<AddressObject>();
            AddableRepository.Add(newObj);
            var makeChangeScript =
                $"$obj = {newObj.ToPsScript()};Add-PANOSObject -ConnectionProperties $ConnectionProperties -FirewallObject $obj";
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
            this.DeletableRepository.Delete(newObj.SchemaName, newObj.Name);    
        }

        [Test]
        public void CommitWhileAnotherCommitIsInProgressTest()
        {
            // Setup
            var newObj = RandomObjectFactory.GenerateRandomObject<AddressObject>();
            AddableRepository.Add(newObj);
            var makeChangeScript =
                $"$obj = {newObj.ToPsScript()};Add-PANOSObject -ConnectionProperties $ConnectionProperties -FirewallObject $obj";
            PsRunner.ExecutePanosPowerShellScript(makeChangeScript);

            // Test
            const string CommitScript = "Save-PANOSChanges -ConnectionProperties $ConnectionProperties";
            PsRunner.ExecutePanosPowerShellScript(CommitScript);
            Assert.That(() => PsRunner.ExecutePanosPowerShellScript(CommitScript), Throws.TypeOf<CmdletInvocationException>());
        }
    }
}
