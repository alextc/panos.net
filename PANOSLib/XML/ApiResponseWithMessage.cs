namespace PANOS
{
    using System.Xml.Serialization;

    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", ElementName = "response", IsNullable = false)]
    public class ApiResponseWithMessage : ApiResponse
    {
        [XmlElement("msg")]
        public string Message { get; set; }
    }
}



