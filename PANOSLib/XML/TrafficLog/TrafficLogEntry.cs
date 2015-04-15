namespace PANOS
{
    using System;
    using System.Text;
    using System.Xml.Serialization;

    [XmlType(AnonymousType = true)]
    public class TrafficLogEntry
    {
        [XmlAttribute("logid")]
        public ulong Logid { get; set; }

        [XmlElement("domain")]
        public uint Domain { get; set; }

        [XmlElement("receive_time")]
        public string ReceiveTime { get; set; }

        // This value does not seem to change
        [XmlElement("serial")]
        public ulong SerialNumber { get; set; }

        [XmlElement("seqno")]
        public UInt32 SequeneceNumber { get; set; }

        [XmlElement("actionflags")]
        public string ActionFlags { get; set; }

        [XmlElement("type")]
        public string Type { get; set; }

        [XmlElement("subtype")]
        public string Subtype { get; set; }

        [XmlElement("config_ver")]
        public UInt32 ConfigVer { get; set; }

        [XmlElement("time_generated")]
        public string TimeGenerated { get; set; }

        [XmlElement("src")]
        public string Source { get; set; }

        [XmlElement("dst")]
        public string Destination { get; set; }

        [XmlElement("rule")]
        public string Rule { get; set; }

        [XmlElement("srcloc")]
        public string SourceLocation { get; set; }

        [XmlElement("dstloc")]
        public string DestinationLocation { get; set; }

        [XmlElement("app")]
        public string App { get; set; }

        [XmlElement("vsys")]
        public string Vsys { get; set; }

        [XmlElement("from")]
        public string From { get; set; }

        [XmlElement("to")]
        public string To { get; set; }

        [XmlElement("inbound_if")]
        public string InboundInterface { get; set; }

        [XmlElement("outbound_if")]
        public string OutboundInterface { get; set; }

        [XmlElement("logset")]
        public string LogSet { get; set; }

        [XmlElement("time_received")]
        public string TimeReceived { get; set; }

        [XmlElement("sessionid")]
        public UInt32 Sessionid { get; set; }

        [XmlElement("repeatcnt")]
        public UInt32 RepeatCount { get; set; }

        [XmlElement("sport")]
        public ushort SourcePort { get; set; }

        [XmlElement("dport")]
        public ushort DestinationPort { get; set; }

        [XmlElement("natsport")]
        public uint NatSourcePort { get; set; }

        [XmlElement("natdport")]
        public uint NatDestinationPort { get; set; }

        [XmlElement("flags")]
        public uint Flags { get; set; }

        [XmlElement("flag-pcap")]
        public string FlagPcap { get; set; }

        [XmlElement("flag-flagged")]
        public string FlagFlagged { get; set; }

        [XmlElement("flag-proxy")]
        public string FlagProxy { get; set; }

        [XmlElement("flag-url-denied")]
        public string FlagUrlDenied { get; set; }

        [XmlElement("flag-nat")]
        public string FlagNat { get; set; }

        [XmlElement("captive-portal")]
        public string CaptivePortal { get; set; }

        [XmlElement("exported")]
        public string Exported { get; set; }

        [XmlElement("transaction")]
        public string Transaction { get; set; }

        [XmlElement("pbf-c2s")]
        public string Pbfc2S { get; set; }

        [XmlElement("pbf-s2c")]
        public string Pbfs2C { get; set; }

        [XmlElement("temporary-match")]
        public string TemporaryMatch { get; set; }

        [XmlElement("sym-return")]
        public string SymReturn { get; set; }

        [XmlElement("decrypt-mirror")]
        public string DecryptMirror { get; set; }

        [XmlElement("proto")]
        public string Protocol { get; set; }

        [XmlElement("action")]
        public string Action { get; set; }

        [XmlElement("cpadding")]
        public uint Cpadding { get; set; }

        [XmlElement("bytes")]
        public UInt64 Bytes { get; set; }

        [XmlElement("bytes_sent")]
        public UInt64 BytesSent { get; set; }

        [XmlElement("bytes_received")]
        public UInt64 BytesReceived { get; set; }

        [XmlElement("packets")]
        public UInt32 Packets { get; set; }

        [XmlElement("start")]
        public string Start { get; set; }

        [XmlElement("elapsed")]
        public UInt32 Elapsed { get; set; }

        [XmlElement("category")]
        public string Category { get; set; }

        [XmlElement("padding")]
        public UInt32 Padding { get; set; }

        [XmlElement("pkts_sent")]
        public UInt32 PacketsSent { get; set; }

        [XmlElement("pkts_received")]
        public UInt32 PacketsReceived { get; set; }

        [XmlElement("session_end_reason")]
        public string SessionEndReason { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendFormat("ReceivedAt: {0}{1}", ReceiveTime, Environment.NewLine);
            sb.AppendFormat("Action: {0}{1}", Action, Environment.NewLine);
            sb.AppendFormat("Source: {0}{1}", Source, Environment.NewLine);
            sb.AppendFormat("Destination: {0}{1}", Destination, Environment.NewLine);
            sb.AppendFormat("Protocol: {0}{1}", Protocol, Environment.NewLine);
            sb.AppendFormat("Rule: {0}{1}", Rule, Environment.NewLine);
            
            return sb.ToString();
        }
    }
}