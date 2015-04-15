namespace PANOS
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Security;

    public class ConfigApiPostKeyValuePairFactory : IConfigApiPostKeyValuePairFactory
    {
        private readonly KeyValuePair<string, string> accessTokenPair;
        private readonly KeyValuePair<string, string> typeConfigPair = new KeyValuePair<string, string>("type", "config");
        private readonly KeyValuePair<string, string> actionShowPair = new KeyValuePair<string, string>("action", "show");
        private readonly KeyValuePair<string, string> actionGetPair = new KeyValuePair<string, string>("action", "get");
        private readonly string vsys;

        public ConfigApiPostKeyValuePairFactory(SecureString accessToken, string vsys = "vsys1")
        {
            accessTokenPair = new KeyValuePair<string, string>("key", SecureStringUtils.ConvertToUnSecureString(accessToken));
            this.vsys = vsys;
        }

        public FormUrlEncodedContent CreateGetSingle(string schemaName, string name, ConfigTypes configType)
        {
            return new FormUrlEncodedContent(new[]
            {
                accessTokenPair,
                typeConfigPair, 
                configType == ConfigTypes.Running ? actionShowPair : actionGetPair, 
                new KeyValuePair<string, string>(
                    "xpath", 
                    string.Format(
                        "/config/devices/entry/vsys/entry[@name='{0}']/{1}/entry[@name='{2}']", 
                            vsys,
                            schemaName,
                            name))
            });
        }

        public FormUrlEncodedContent CreateDelete(string schemName, string name)
        {
            return new FormUrlEncodedContent(new[]
            {
                accessTokenPair,
                typeConfigPair, 
                new KeyValuePair<string, string>("action", "delete"),
                new KeyValuePair<string, string>(
                    "xpath", 
                    string.Format(
                        "/config/devices/entry/vsys/entry[@name='{0}']/{1}/entry[@name='{2}']", 
                            vsys,
                            schemName,
                            name))
            });
        }

        public FormUrlEncodedContent CreateRename(string schemaName, string oldName, string newName)
        {
            return new FormUrlEncodedContent(new[]
            {
                accessTokenPair,
                typeConfigPair, 
                new KeyValuePair<string, string>("action", "rename"),
                new KeyValuePair<string, string>(
                    "xpath", 
                    string.Format(
                        "/config/devices/entry/vsys/entry[@name='{0}']/{1}/entry[@name='{2}']", 
                            vsys,
                            schemaName,
                            oldName)),
                new KeyValuePair<string, string>("newname", newName)
            });
        }

        public FormUrlEncodedContent CreateGetAll(string schemaName, ConfigTypes configType)
        {
            return new FormUrlEncodedContent(new[]
            {
                accessTokenPair,
                typeConfigPair, 
                configType == ConfigTypes.Running ? actionShowPair : actionGetPair, 
                new KeyValuePair<string, string>(
                    "xpath", 
                    string.Format(
                        "/config/devices/entry/vsys/entry[@name='{0}']/{1}", 
                            vsys,
                            schemaName))
            });
        }


        // It is possible  to call the resulting command against an existing object, in such a case the prior object will be update, but see below for a special case.
        // // For objects that have a concept of a membership and in cases where the resulting commmand is called against an existing object,
        // such command will only be able to add members, but NOT remove them.
        // Use CreateSetMembership to deal with Membership changes
        public FormUrlEncodedContent CreateSet(FirewallObject firewallObject)
        {
            return new FormUrlEncodedContent(new[]
            {
                accessTokenPair,
                typeConfigPair, 
                new KeyValuePair<string, string>("action", "set"),
                new KeyValuePair<string, string>(
                    "xpath", 
                    string.Format(
                        "/config/devices/entry/vsys/entry[@name='{0}']/{1}/entry[@name='{2}']", 
                            vsys,
                            firewallObject.SchemaName,
                            firewallObject.Name)),
                new KeyValuePair<string, string>("element", firewallObject.ToXml()), 
            });
        }
        
        public FormUrlEncodedContent CreateSetMembership(GroupFirewallObject groupFirewallObject)
        {
            return new FormUrlEncodedContent(new[]
            {
                accessTokenPair,
                typeConfigPair, 
                new KeyValuePair<string, string>("action", "edit"),
                new KeyValuePair<string, string>(
                    "xpath", 
                    string.Format(
                        "/config/devices/entry/vsys/entry[@name='{0}']/{1}/entry[@name='{2}']", 
                            vsys,
                            groupFirewallObject.SchemaName,
                            groupFirewallObject.Name)),
                new KeyValuePair<string, string>("element", groupFirewallObject.StaticMembershipSetRequestAsXml())
            });
        }

        //private static string GetSchemaName(MemberInfo type)
        //{
        //    // xmlRootAttribute contains the name of the schema object, ex [XmlRoot("address")]
        //    var xmlRootAttribute = (XmlRootAttribute)Attribute.GetCustomAttribute(type, typeof(XmlRootAttribute), false);
        //    return xmlRootAttribute.ElementName;
        //}
    }
}
