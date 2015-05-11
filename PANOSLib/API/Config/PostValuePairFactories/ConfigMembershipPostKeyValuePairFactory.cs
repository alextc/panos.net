namespace PANOS
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Security;

    public class ConfigMembershipPostKeyValuePairFactory : IConfigMembershipPostKeyValuePairFactory
    {
        private readonly KeyValuePair<string, string> accessTokenPair;
        private readonly KeyValuePair<string, string> typeConfigPair = new KeyValuePair<string, string>("type", "config");
        private readonly string vsys;

        public ConfigMembershipPostKeyValuePairFactory(SecureString accessToken, string vsys)
        {
            accessTokenPair = new KeyValuePair<string, string>("key", SecureStringUtils.ConvertToUnSecureString(accessToken));
            this.vsys = vsys;
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
    }
}
