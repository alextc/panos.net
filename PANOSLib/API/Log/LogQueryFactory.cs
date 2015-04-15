namespace PANOS
{
    using System;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;

    public class LogQueryFactory
    {
        // ( action eq deny) and ( addr.src in 10.127.63.254 ) and ( receive_time leq '2015/03/16 12:46:23' ) and (receive_time geq '2015/03/16 00:45:00')

        const string PanosRecieveTimeLeqClauseFormatWithBrackets = "(receive_time leq '{0:yyyy/MM/dd HH:mm:ss}')";
        const string PanosRecieveTimeLeqClauseFormatWithoutBrackets = "receive_time leq '{0:yyyy/MM/dd HH:mm:ss}'";
        const string PanosRecieveTimeGeqClauseFormat = "(receive_time geq '{0:yyyy/MM/dd HH:mm:ss}')";
        const string ReceiveTimeInClausePattern = "(receive_time in last-)";
        // What about upper case? Recieve_Time ?
        private const string ReceiveTimeClausePattern = @"receive_time\s+leq\s+'\d{4}\/\d{2}\/\d{2}\s{1}\d{2}:\d{2}:\d{2}'";
        
        public string CreateGetBlockedTrafficFromSourceWithinTimeRange(IPAddress source, DateTime lowerBound, DateTime upperBound)
        {
            var sb = new StringBuilder();
            sb.Append("(action eq deny)");
            sb.Append(" and ");
            sb.AppendFormat("(addr.src in {0})", source);
            sb.Append(" and ");
            sb.AppendFormat(PanosRecieveTimeLeqClauseFormatWithBrackets, upperBound);
            sb.Append(" and ");
            sb.AppendFormat(PanosRecieveTimeGeqClauseFormat, lowerBound);
            return sb.ToString();
        }

        public string CreateGetTrafficWithUpperTimeRange(DateTime upperBound)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(PanosRecieveTimeLeqClauseFormatWithBrackets, upperBound);
            return sb.ToString();
        }

        public bool IsUpperTimeRangeConditionPresent(string query)
        {
            var regex = new Regex(ReceiveTimeClausePattern);
            return regex.Match(query).Success;
        }

        public bool IsUpperTimeInConditionPresent(string query)
        {
            var regex = new Regex(ReceiveTimeInClausePattern);
            return regex.Match(query).Success;
        }

        public string AppendTrafficWithUpperTimeRange(string query, DateTime upperBound)
        {
            var regex = new Regex(ReceiveTimeClausePattern);
            if (regex.Match(query).Success)
            {
                throw new ArgumentException("Supplied query already contains recieve_time leq condition.");
            }

            var sb = new StringBuilder();
            sb.Append(query);
            sb.Append(" and ");
            sb.AppendFormat(PanosRecieveTimeLeqClauseFormatWithBrackets, upperBound);
            return sb.ToString();
        }

        public string UpdateGetTrafficWithUpperTimeRange(string query, DateTime upperBound)
        {
            var regex = new Regex(ReceiveTimeClausePattern);
            if (regex.Match(query).Success)
            {
                var replacement = string.Format(PanosRecieveTimeLeqClauseFormatWithoutBrackets, upperBound);
                return regex.Replace(query, replacement);
            }
            
            throw new ArgumentException("Supplied query does not contain recieve_time leq condition.");
        }
    }
}
