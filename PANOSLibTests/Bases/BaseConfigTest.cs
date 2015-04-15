namespace PANOSLibTest
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PANOS;

    public class BaseConfigTest : BaseTest
    {
        public IConfigCommandFactory ConfigCommandFactory { get; set; }

        public ICommitCommandFactory CommitCommandFactory { get; set; }

        public IConfigRepository ConfigRepository { get; private set; }

        public RandomObjectFactory RandomObjectFactory { get; set; }

        protected BaseConfigTest()
        {
            this.ConfigCommandFactory = 
                new ConfigCommandFactory(
                    new ApiUriFactory(
                        ConnectionProperties.Host), 
                    new ConfigApiPostKeyValuePairFactory(
                        ConnectionProperties.AccessToken,
                        ConnectionProperties.Vsys));

            this.CommitCommandFactory = new CommitApiCommandFactory(
                new ApiUriFactory(ConnectionProperties.Host),
                new CommitApiPostKeyValuePairFactory(ConnectionProperties.AccessToken));

            this.ConfigRepository = new ConfigRepository(this.ConfigCommandFactory);

            RandomObjectFactory = new RandomObjectFactory(this.ConfigRepository);
        }

        protected void CommitCandidateConfig(bool waitForCompletion = true)
        {
            var commitQueryResonse = this.CommitCommandFactory.CreateCommit(true).Execute();
            Assert.IsTrue(commitQueryResonse.Status.Equals("success"));
            Assert.AreEqual(commitQueryResonse.Code, (byte)CommitStatus.Success);
            Debug.WriteLine("CommitApi completed successfully, check job ID {0}", commitQueryResonse.Result.JobId);
            if (waitForCompletion)
            {
                Thread.Sleep(TimeSpan.FromSeconds(90)); 
            }
        }
    }
}
