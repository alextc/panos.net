namespace PANOS
{
    using System;
    using System.Net;
    using System.Text;

    public class AddressObject : FirewallObject, IEquatable<AddressObject>
    {
        public AddressObject(string name, IPAddress ipAddress, string description = "", string tag = "") : base(name, "address", description)
        {
            Address = ipAddress;
        }

        // ReSharper disable once MemberCanBeProtected.Global
        // This needs to be public, otherwise not visible in a PS session
        public IPAddress Address { get; set; }

        public override string ToXml()
        {
            // TODO: Add description and Tag
            return $"<ip-netmask>{Address}</ip-netmask>";
        }

        public override string ToPsScript()
        {
            return $"New-Object -TypeName 'PANOS.AddressObject' -ArgumentList '{Name}', '{Address}';";
        }

        public override void Mutate()
        {
            Address = new RandomAddressObjectFactory().GenerateRandomIpAddress();
        }

        public bool Equals(AddressObject other)
        {
            return Address.Equals(other.Address) && 
                   Name.Equals(other.Name, StringComparison.InvariantCulture);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}, Address:{1}", base.ToString(), this.Address);
            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            var addressObject = obj as AddressObject;
            if (addressObject == null)
            {
                return false;
            }

            return Address.Equals(addressObject.Address) && Name.Equals(addressObject.Name, StringComparison.InvariantCulture);
        }

        public override int GetHashCode()
        {
            var hashAddressName = Name.GetHashCode();
            var hashIpAddress = Address.GetHashCode();
            return hashAddressName ^ hashIpAddress;
        }
    }
}
