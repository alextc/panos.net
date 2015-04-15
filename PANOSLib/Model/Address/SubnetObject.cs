namespace PANOS
{
    using System;
    using System.Collections;
    using System.Net;
    using System.Text;

    public class SubnetObject : AddressObject
    {
        public SubnetObject(string name, IPAddress ipAddress, uint subnetMask, string description = "", string tag = "")
            : base(name, ipAddress, description)
        {
            ValidateSubnet(ipAddress, subnetMask);
            this.SubnetMask = subnetMask; 
        }

        public uint SubnetMask { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}, Mask:{1}", base.ToString(), this.SubnetMask);
            return sb.ToString();
        }

        public override string ToXml()
        {
            // TODO: Add description and Tag
            return string.Format(
                "<ip-netmask>{0}/{1}</ip-netmask>", this.Address, this.SubnetMask);
        }

        public override void Mutate()
        {
            Address = RandomObjectFactory.GenerateRandomIpSubnetAddress();
            SubnetMask = 24; // RandomSubnet is always /24
        }

        public override string ToPsScript()
        {
            return string.Format("New-Object -TypeName 'PANOS.SubnetObject' -ArgumentList '{0}', '{1}', {2};", this.Name, this.Address, this.SubnetMask);
        }

        public override bool Equals(object obj)
        {
            var subnetObject = obj as SubnetObject;
            if (subnetObject == null)
            {
                return false;
            }

            return this.Address.Equals(subnetObject.Address) && this.Name.Equals(subnetObject.Name) && this.SubnetMask.Equals(subnetObject.SubnetMask);
        }

        // http://stackoverflow.com/questions/461742/how-to-convert-an-ipv4-address-into-a-integer-in-c
        // The reason for such a complicated logic is due to the fact that bits in the individual octets need to be reversed 
        private static void ValidateSubnet(IPAddress ipAddress, uint subnetMask)
        {
            var addressBytes = ipAddress.GetAddressBytes();
            Array.Reverse(addressBytes);
            var addressBits = new BitArray(addressBytes);

            if (addressBits.Count <= subnetMask || subnetMask == 0)
            {
                throw new ArgumentException("Invalid subnet mask");
            }
            
            // Array is reversed so the last octed in the IP goes first
            for (var i = 0; i < (addressBits.Count - subnetMask); i++)
            {
                if (addressBits[i])
                {
                    throw new ArgumentException("Invalid subnet");
                }
            }
        }
    }
}
