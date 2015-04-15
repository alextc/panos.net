namespace PANOS
{
    using System.Xml.Serialization;

    [XmlType(AnonymousType = true)]
    public class Message
    {
        [XmlElement("line")]
        public string Line { get; set; }
    }
}
