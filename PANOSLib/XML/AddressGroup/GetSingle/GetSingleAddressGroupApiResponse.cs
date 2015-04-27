namespace PANOS
{
    using System.Xml.Serialization;

    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", ElementName = "response", IsNullable = false)]
    public class GetSingleAddressGroupApiResponse : ApiResponseForGetSingle
    {
        [XmlElement("result")]
        public GetSingleAddressGroupApiResponseResult Result { get; set; }

        public override Maybe<FirewallObject> GetPayload()
        {
            return Result.AddressGroupXml != null ? 
                new Maybe<FirewallObject>(Result.AddressGroupXml.AddressGroupObject) : 
                new Maybe<FirewallObject>();
        }
    }
}
