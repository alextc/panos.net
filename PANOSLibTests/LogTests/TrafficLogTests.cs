namespace PANOSLibTest
{
    using NUnit.Framework;

    [TestFixture]
    public class TrafficLogTests : BaseLogTest
    {
        [Test]
        public void GetTrafficLogNoPagingTest()
        {
            int numberOfRoundTrips = 0;
            foreach (var subResult in this.LogRepository.GetTrafficLog("", false, 4))
            {
                Assert.IsNotNull(subResult);
                Assert.AreEqual(subResult.Count, 5000);
                numberOfRoundTrips++;
            }

            Assert.AreEqual(numberOfRoundTrips, 1);
        }

        [Test]
        public void GetTrafficLogNoPagedTest()
        {
            int numberOfRoundTrips = 0;
            int totalNumberOfRecords = 0;
            foreach (var subResult in this.LogRepository.GetTrafficLog("", true, 4))
            {
                Assert.IsNotNull(subResult);
                // Provisioning for the overalap between requests
                Assert.IsTrue(subResult.Count > 4900);
                numberOfRoundTrips++;
                totalNumberOfRecords = totalNumberOfRecords + subResult.Count;
                if (numberOfRoundTrips == 3)
                {
                    break;
                }
            }

            Assert.AreEqual(numberOfRoundTrips, 3);
            Assert.IsTrue(totalNumberOfRecords >  14000);
        }
    }
}
