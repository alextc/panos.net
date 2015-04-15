namespace PANOS
{
    using System.Xml.Serialization;

    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", ElementName = "response", IsNullable = false)]
    public class GetSingleAddressApiResponse : ApiResponse, IPayload
    {
        [XmlElement("result")]
        public GetSingleAddressApiResponseResult Result { get; set; }

        public FirewallObject GetPayload()
        {
            return Result.AddressXml != null ? Result.AddressXml.GetPayload() : null;
        }
    }  
}
