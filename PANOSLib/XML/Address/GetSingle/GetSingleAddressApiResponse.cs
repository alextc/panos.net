namespace PANOS
{
    using System.Xml.Serialization;

    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", ElementName = "response", IsNullable = false)]
    public class GetSingleAddressApiResponse : ApiResponseForGetSingle
    {
        [XmlElement("result")]
        public GetSingleAddressApiResponseResult Result { get; set; }

        public override Maybe<FirewallObject> GetPayload()
        {
            return Result.AddressXml != null ? new Maybe<FirewallObject>(Result.AddressXml.GetPayload()) : new Maybe<FirewallObject>();
        }
    }  
}
