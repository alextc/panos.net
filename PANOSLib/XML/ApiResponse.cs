namespace PANOS
{
    using System.Xml.Serialization;

    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", ElementName = "response", IsNullable = false)]
    public abstract class ApiResponse
    {
        [XmlAttributeAttribute("status")]
        public string Status { get; set; }

        [XmlAttributeAttribute("code")]
        public int Code { get; set; }
    }
}
