namespace PANOS
{
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;

    [XmlTypeAttribute(AnonymousType = true)]
    public class ApiEnqueuedResponseResult
    {
        [XmlElement("msg")]
        public Message Message { get; set; }

        [XmlAttribute("job")]
        private uint Job { get; set; }

        [XmlIgnore]
        public uint JobId => uint.Parse(Regex.Match(Message.Line, "[0-9]+").Value);
    }
}
