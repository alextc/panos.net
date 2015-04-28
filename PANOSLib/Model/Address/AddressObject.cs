namespace PANOS
{
    using System;
    using System.Net;
    using System.Text;

    public class AddressObject : FirewallObject, IEquatable<AddressObject>
    {
        public AddressObject(string name, IPAddress ipAddress, string description = "", string tag = "") : base(name, "address", description)
        {
            this.Address = ipAddress;
        }

        
        // ReSharper disable once MemberCanBeProtected.Global
        // This needs to be public, otherwise not visible in a PS session
        public IPAddress Address { get; set; }

        public override string ToXml()
        {
            // TODO: Add description and Tag
            return string.Format(
                "<ip-netmask>{0}</ip-netmask>", this.Address);
        }

        public override string ToPsScript()
        {
            return string.Format("New-Object -TypeName 'PANOS.AddressObject' -ArgumentList '{0}', '{1}';", this.Name, this.Address);
        }

        public override void Mutate()
        {
            Address = new RandomAddressObjectFactory().GenerateRandomIpAddress();
        }

        public bool Equals(AddressObject other)
        {
            return this.Address.Equals(other.Address) && 
                   this.Name.Equals(other.Name, StringComparison.InvariantCultureIgnoreCase);
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

            return this.Address.Equals(addressObject.Address) && this.Name.Equals(addressObject.Name, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            var hashAddressName = Name.GetHashCode();
            var hashIpAddress = Address.GetHashCode();
            return hashAddressName ^ hashIpAddress;
        }
    }
}
