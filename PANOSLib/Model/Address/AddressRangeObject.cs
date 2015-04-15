namespace PANOS
{
    using System;
    using System.Net;
    using System.Text;

    public class AddressRangeObject : FirewallObject
    {
        public AddressRangeObject(string name, IPAddress rangeStartAddress, IPAddress rangeEndAddress, string description = "", string tag = "")
            : base(name, "address", description)
        {
            ValidateSubnet(rangeStartAddress, rangeEndAddress);
            this.RangeStartAddress = rangeStartAddress;
            this.RangeEndAddress = rangeEndAddress;
        }

        public IPAddress RangeStartAddress { get; set; }

        public IPAddress RangeEndAddress { get; set; }

        public override string ToXml()
        {
            // TODO: Add description and Tag
            return string.Format(
               "<ip-range>{0}-{1}</ip-range>", this.RangeStartAddress, this.RangeEndAddress);
        }

        public override string ToPsScript()
        {
            return string.Format("New-Object -TypeName 'PANOS.AddressRangeObject' -ArgumentList '{0}', '{1}', {2};", 
                this.Name, 
                this.RangeStartAddress,
                this.RangeEndAddress);
        }

        public override void Mutate()
        {
            var randomRange = RandomObjectFactory.GenerateRandomAddressRange();
            RangeStartAddress = randomRange.RangeStartAddress;
            RangeEndAddress = randomRange.RangeEndAddress;
        }

        public override bool Equals(object obj)
        {
            var addressRangeObject = obj as AddressRangeObject;
            if (addressRangeObject == null)
            {
                return false;
            }

            return 
                this.Name.Equals(addressRangeObject.Name) &&
                this.RangeStartAddress.Equals(addressRangeObject.RangeStartAddress) &&
                this.RangeEndAddress.Equals(addressRangeObject.RangeEndAddress);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}, RangeStartAddress:{1}, RangeEndAddress:{2}", base.ToString(), this.RangeStartAddress, this.RangeEndAddress);
            return sb.ToString();
        }

        // http://stackoverflow.com/questions/461742/how-to-convert-an-ipv4-address-into-a-integer-in-c
        // The reason for such a complicated logic is due to the fact that bits in the individual octets need to be reversed 
        private static void ValidateSubnet(IPAddress ipAddressRangeStart, IPAddress ipAddressRangeEnd)
        {
            var addressRangeStartBytes = ipAddressRangeStart.GetAddressBytes();
            Array.Reverse(addressRangeStartBytes);
            var addressRangeEndBytes = ipAddressRangeEnd.GetAddressBytes();
            Array.Reverse(addressRangeEndBytes);

            var addressRangeStartAsInt = BitConverter.ToUInt32(addressRangeStartBytes, 0);
            var addressRangeEndAsInt = BitConverter.ToUInt32(addressRangeEndBytes, 0);

            if (addressRangeStartAsInt >= addressRangeEndAsInt)
            {
                throw new ArgumentException("Invalid Range, start must be less than end");
            }
        }
    }
}
