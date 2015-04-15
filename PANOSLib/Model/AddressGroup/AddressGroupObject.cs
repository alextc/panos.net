namespace PANOS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class AddressGroupObject : GroupFirewallObject
    {
        public AddressGroupObject(string name, List<string> members,  string description = "", string tag = "")
            : base(name, "address-group", members, description)
        {
            Members = members;
        }

        public AddressGroupObject(string name, string[] members, string description = "", string tag = "")
            : base(name, "address-group", members.ToList(), description)
        {
            Members = new List<string>(members);
        }

        public override string ToXml()
        {
            var sb = new StringBuilder();
            sb.Append("<static>");
            foreach (var member in Members)
            {
                sb.AppendFormat("<member>{0}</member>", member);
            }
            sb.Append("</static>");
            return sb.ToString();
        }

        public override string ToPsScript()
        {
            var script =  string.Format("New-PANOSAddressGroup -Name '{0}' -Members {1};",
                this.Name, string.Join(",", this.Members));
            return script;
        }

        public override void Mutate()
        {
            var rnd = new Random();
            var id = rnd.Next(0, Members.Count - 1);
            Members[id] += rnd.Next();
        }

        // Could this be moved-up to parent?
        public override bool Equals(object obj)
        {
            var addressGroupObject = obj as AddressGroupObject;
            if (addressGroupObject == null)
            {
                return false;
            }

            return Name.Equals(addressGroupObject.Name) && Members.OrderBy(m => m).SequenceEqual(addressGroupObject.Members.OrderBy(m => m));
        }

        // Could this be moved-up to parent?
        public bool DeepCompare(AddressGroupObject target)
        {
            if ((MemberObjects.Count != Members.Count) || (target.MemberObjects.Count != target.Members.Count))
            {
                throw new ArgumentException("Attempt to call DeepCompare method on an object prior to inflating the membership");
            }

            return this.Equals(target) && 
                   this.MemberObjects.OrderBy(a => a.Name).SequenceEqual(target.MemberObjects.OrderBy(a => a.Name));
        }


        // Note on Except: The set difference of two sets is defined as the members of the first set that do not appear in the second set.
        // In other words, this will return all member objects in this object that are not found in target
        public List<FirewallObject> GetDelta(AddressGroupObject target) 
        {
            if (DeepCompare(target))
            {
                return null;
            }

            return MemberObjects.Except(target.MemberObjects).ToList();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(base.ToString());
            sb.AppendLine("Members");
            foreach (var member in Members.OrderBy(m => m))
            {
                sb.AppendLine(member);
            }

            return sb.ToString();
        }
    }
}
