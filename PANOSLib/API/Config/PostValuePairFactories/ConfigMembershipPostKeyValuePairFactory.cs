﻿namespace PANOS
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Security;

    // private const string RemoveMemberQueryTemplate = "{0}&type=config&action=delete&xpath=/config/devices/entry/vsys/entry[@name='{1}']/{2}/entry[@name='{3}']/static/member[text
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

        public FormUrlEncodedContent CreateAddMember(string groupName, string schemaName, string memberName)
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
                                schemaName,
                                groupName)),
                    new KeyValuePair<string, string>("element", string.Format("<static><member>{0}</member></static>", memberName))
            });
        }
    }
}
