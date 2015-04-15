namespace PANOS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Xml.Serialization;

    [XmlType(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", ElementName = "response", IsNullable = false)]
    public class GetTrafficLogApiResponse : ApiResponse
    {
        [XmlElement("result")]
        public GetTrafficLogApiResponseResult Result { get; set; }

        [XmlIgnore]
        public List<TrafficLogEntryObject> PayLoad
        {
            get
            {
                if (Result.LogsContainer.Logs.TrafficLogEntries != null)
                {
                    return
                    new List<TrafficLogEntryObject>(
                        Result.LogsContainer.Logs.TrafficLogEntries.Select(
                            l => new TrafficLogEntryObject(
                                l.SerialNumber,
                                l.Logid,
                                l.Vsys,
                                DateTime.Parse(l.ReceiveTime),
                                IPAddress.Parse(l.Source),
                                IPAddress.Parse(l.Destination),
                                l.App,
                                l.Action,
                                l.Rule,
                                l.DestinationPort,
                                l.From,
                                l.To,
                                l.Bytes)));
                }

                return null;
            }
        }
    }

    [XmlType(AnonymousType = true)]
    public class GetTrafficLogApiResponseResult
    {
        [XmlElement("job")]
        public TrafficLogJob Job { get; set; }

        [XmlElement("log")]
        public TrafficLogsContainer LogsContainer { get; set; }
    }

    [XmlType(AnonymousType = true)]
    public class TrafficLogJob
    {
        [XmlElement(ElementName = "tenq", DataType = "time")]
        public DateTime TimeEnqueued { get; set; }

        [XmlElement(ElementName = "tdeq", DataType = "time")]
        public DateTime TimeDequeued { get; set; }

        [XmlElement(ElementName = "tlast", DataType = "time")]
        public DateTime TimeLast { get; set; }

        [XmlElement("status")]
        public string Status { get; set; }

        [XmlElement("id")]
        public UInt32 Id { get; set; }
    }

    [XmlType(AnonymousType = true)]
    public class TrafficLogsContainer
    {
        [XmlElement("logs")]
        public TrafficLog Logs { get; set; }
    }

    [XmlType(AnonymousType = true)]
    public class TrafficLog
    {
        [XmlElement("entry")]
        public TrafficLogEntry[] TrafficLogEntries { get; set; }

        [XmlAttribute("count")]
        public UInt32 Count { get; set; }

        [XmlAttribute("progress")]
        public byte Progress { get; set; }
    }
}
