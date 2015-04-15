namespace PANOS
{
    using System;
    using System.Net;
    using System.Text;

    public class TrafficLogEntryObject 
    {
        public DateTime ReceiveTime { get; set; }

        public IPAddress Source { get; set; }

        public IPAddress Destination { get; set; }

        public string Rule { get; set; }

        public string App { get; set; }

        public ushort DestinationPort { get; set; }

        public string Action { get; set; }

        public string SourceHostName { get; set; }

        public string DestinationHostName { get; set; }

        public ulong FwSerialNumber { get; set; }

        public ulong Id { get; set; }
        
        public string Vsys { get; set; }

        public string FromZone { get; set; }

        public string ToZone { get; set; }

        public ulong Bytes { get; set; }

       public TrafficLogEntryObject(
            ulong fwSerialNumber,
            ulong id,
            string vsys,
            DateTime receiveTime,
            IPAddress source,
            IPAddress destination,
            string app,
            string action,
            string rule,
            ushort destinationPort,
            string fromZone,
            string toZone,
            ulong bytes)
            
        {
            ReceiveTime = receiveTime;
            Source = source;
            Destination = destination;
            App = app;
            Action = action;
            Rule = rule;
            DestinationPort = destinationPort;
            FwSerialNumber = fwSerialNumber;
            Id = id;
            Vsys = vsys;
            FromZone = fromZone;
            ToZone = toZone;
            Bytes = bytes;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendFormat("ReceivedAt: {0}{1}", ReceiveTime, Environment.NewLine);
            sb.AppendFormat("Action: {0}{1}", Action, Environment.NewLine);
            sb.AppendFormat("Source: {0}{1}", Source, Environment.NewLine);
            sb.AppendFormat("Destination: {0}{1}", Destination, Environment.NewLine);
            sb.AppendFormat("Application: {0}{1}", App, Environment.NewLine);
            sb.AppendFormat("Rule: {0}{1}", Rule, Environment.NewLine);

            return sb.ToString();
        }
    }
}
