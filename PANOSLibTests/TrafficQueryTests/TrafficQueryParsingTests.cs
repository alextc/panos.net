namespace PANOSLibTest.TrafficQueryTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using PANOS;

    // PANOS DateTime pattern 2015/03/25 16:29:02

    [TestClass]
    public class TrafficQueryParsingTests
    {
        // private readonly List<string> timeBasedAttributes = new List<string> { "receive_time", "time_generated " };
        // private List<string> timeBasedOperators = new List<string> {"in last", "leq", "geq"};

        // this will match something like 9999/99/99 but this is good enough for my purposes - I trust PANOS to format dates correctlty.
        private const string ReceiveTimeClausePattern = @"receive_time\s+leq\s+'\d{4}\/\d{2}\/\d{2}\s{1}\d{2}:\d{2}:\d{2}'";
        const string PanosRecieveTimeLeqClauseFormat = "(receive_time leq '{0:yyyy/MM/dd HH:mm:ss}')";
        
        
        [TestMethod]
        public void FindReceiveTimeClausesTest()
        {
            const string QueryWithBothTimeAttributes =
                "( addr.src in 10.121.148.166 )  and ( action eq deny ) and ( receive_time leq '2015/03/25 16:29:02' ) or ( receive_time leq '2015/02/25 16:29:02' )";
            
            var recieveTimeClauseRegex = new Regex(ReceiveTimeClausePattern);
            var recieveTimeClauseMatches = new List<Match>();
            var recieveDateTimes = new List<DateTime>();
            var recieveTimeClauseMatch = recieveTimeClauseRegex.Match(QueryWithBothTimeAttributes);
            while (recieveTimeClauseMatch.Success)
            {
                recieveTimeClauseMatches.Add(recieveTimeClauseMatch);
                var receiveDateTime = ExtractDateTimeFromRecieveTimeClause(recieveTimeClauseMatch.Value);
                Assert.IsNotNull(receiveDateTime);
                recieveDateTimes.Add(receiveDateTime);
                recieveTimeClauseMatch = recieveTimeClauseMatch.NextMatch();
            }

            Assert.AreEqual(recieveTimeClauseMatches.Count, 2);
            Assert.AreEqual(recieveTimeClauseMatches[0].Value, "receive_time leq '2015/03/25 16:29:02'");
            Assert.AreEqual(recieveDateTimes[0], new DateTime(2015, 3, 25, 16, 29, 02));
            Assert.AreEqual(recieveTimeClauseMatches[1].Value, "receive_time leq '2015/02/25 16:29:02'");
            Assert.AreEqual(recieveDateTimes[1], new DateTime(2015, 2, 25, 16, 29, 02));
        }

        [TestMethod]
        public void TimeRecieveUpdateTests()
        {
            const string QueryWithBothTimeAttributes =
                "( addr.src in 10.121.148.166 )  and ( action eq deny ) and ( receive_time leq '2015/03/25 16:29:02' ) or ( receive_time leq '2015/02/25 16:29:02' )";
            var queryFactory = new LogQueryFactory();
            var newDateTime = DateTime.Now;
            var result = queryFactory.UpdateGetTrafficWithUpperTimeRange(QueryWithBothTimeAttributes, DateTime.Now);
            Assert.IsTrue(result.Contains(string.Format(PanosRecieveTimeLeqClauseFormat, newDateTime)));
        }

       
        private static DateTime ExtractDateTimeFromRecieveTimeClause(string recieveTimeClauseMatch)
        {
            const string PanosDateTimeFormatPattern = @"\d{4}\/\d{2}\/\d{2}\s{1}\d{2}:\d{2}:\d{2}";
            var panosDateTimeRegex = new Regex(PanosDateTimeFormatPattern);
            var match = panosDateTimeRegex.Match(recieveTimeClauseMatch);
            if (match.Success)
            {
                DateTime dateTime;
                DateTime.TryParse(match.Value, out dateTime);
                return dateTime;
            }
           
            throw new Exception("Unable to extract DateTime from Recieve Clause");
        }
    }
}
