namespace PANOS
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public abstract class GroupFirewallObject : FirewallObject
    {
        public List<String> Members { get; set; }

        public List<FirewallObject> MemberObjects { get; set; } 

        protected GroupFirewallObject(string name, string schemName, List<string> members, string description)
            : base(name, schemName, description)
        {
            Members = members;
            MemberObjects = new List<FirewallObject>();
        }

        public string StaticMembershipSetRequestAsXml()
        {
            var membershipBuilder = new StringBuilder();
            membershipBuilder.AppendFormat("<entry name='{0}'>", Name);
            membershipBuilder.Append("<static>");
            foreach (var member in Members)
            {
                membershipBuilder.AppendFormat("<member>{0}</member>", member);
            }
            membershipBuilder.Append("</static>");
            membershipBuilder.Append("</entry>");

            return membershipBuilder.ToString();
        }
    }
}
