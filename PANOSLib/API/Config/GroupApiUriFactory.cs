/*
namespace PANOS
{
    using System;
    using System.Text;

    // This class will be refactored into FirewallAddressObject
    public abstract class GroupApiUriFactory : FirewallObjectCommandFactory
    {
        private const string AddMemberQueryTemplate = "{0}&type=config&action=set&xpath=/config/devices/entry/vsys/entry[@name='{1}']/{2}/entry[@name='{3}']&element=<static><member>{4}</member></static>";

        private const string RemoveMemberQueryTemplate = "{0}&type=config&action=delete&xpath=/config/devices/entry/vsys/entry[@name='{1}']/{2}/entry[@name='{3}']/static/member[text()='{4}']";

        protected GroupApiUriFactory(ConnectionProperties ConnectionProperties) : 
            base(ConnectionProperties)
        { }

        protected Uri BuildAddMemberQuery(FirewallObject firewallObject, string memberName)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(
                AddMemberQueryTemplate,
                this.ConnectionProperties.ToUri(),
                this.ConnectionProperties.Vsys,
                firewallObject.SchemaName,
                firewallObject.Name,
                memberName);

            Uri result;
            if (Uri.TryCreate(sb.ToString(), UriKind.Absolute, out result))
            {
                return result;
            }

            throw new Exception(string.Format("Unable to create Uri from {0}", sb));
        }

        protected Uri BuildRemoveMemberQuery(FirewallObject firewallObject, string memberName)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(
                RemoveMemberQueryTemplate,
                this.ConnectionProperties.ToUri(),
                this.ConnectionProperties.Vsys,
                firewallObject.SchemaName,
                firewallObject.Name,
                memberName);

            Uri result;
            if (Uri.TryCreate(sb.ToString(), UriKind.Absolute, out result))
            {
                return result;
            }

            throw new Exception(string.Format("Unable to create Uri from {0}", sb));
        }
    }
}
 */
