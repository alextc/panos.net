﻿namespace PANOSLibTest
{
    using System;
    using System.Net;

    using NUnit.Framework;

    using PANOS;

    [TestFixture]
    public class LogQueryFactoryTests
    {
        // ( action eq deny) and ( addr.src in 10.127.63.254 ) and ( receive_time leq '2015/03/16 12:46:23' ) and (receive_time geq '2015/03/16 00:45:00')

        [Test]
        public void DroppedTrafficFromSourceInTimeRangeTest()
        {
            var logQueryFactory = new LogQueryFactory();
            var query = logQueryFactory.CreateGetBlockedTrafficFromSourceWithinTimeRange(
                IPAddress.Parse("10.0.10.5"),
                DateTime.Now.AddHours(-4),
                DateTime.Now);

            Assert.IsFalse(string.IsNullOrEmpty(query));
        }
    }
}
