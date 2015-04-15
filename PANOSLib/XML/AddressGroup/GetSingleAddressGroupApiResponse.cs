namespace PANOS
{
    using System.Xml.Serialization;

    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", ElementName = "response", IsNullable = false)]
    public class GetSingleAddressGroupApiResponse : ApiResponse, IPayload
    {
        [XmlElement("result")]
        public GetSingleAddressGroupApiResponseResult Result { get; set; }

        public FirewallObject GetPayload()
        {
            return Result.AddressGroupXml != null ? Result.AddressGroupXml.AddressGroupObject : null;
        }
    }
}
