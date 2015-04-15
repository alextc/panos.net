namespace PANOSLibTest.Deserealization
{
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PANOS;

    [TestClass]
    public class DeserealizationTests
    {
        [TestMethod]
        public void DeserealizeTrafficLog()
        {
            #region sampleData
            const string Xml =
                    @"<response status=""success"">
	                        <result>
		                        <job>
			                        <tenq>15:04:15</tenq>
			                        <tdeq>15:04:15</tdeq>
			                        <tlast>15:04:15</tlast>
			                        <status>FIN</status>
			                        <id>29972</id>
		                        </job>
		                        <log>
			                        <logs count=""1"" progress=""100"">
				                        <entry logid=""6125502973398933156"">
					                        <domain>1</domain>
					                        <receive_time>2015/03/13 15:04:13</receive_time>
					                        <serial>007901000712</serial>
					                        <seqno>241120890</seqno>
					                        <actionflags>0x0</actionflags>
					                        <type>TRAFFIC</type>
					                        <subtype>deny</subtype>
					                        <config_ver>1</config_ver>
					                        <time_generated>2015/03/13 15:04:14</time_generated>
					                        <src>10.193.160.78</src>
					                        <dst>23.12.33.16</dst>
					                        <rule>Logging Rule</rule>
					                        <srcloc>10.0.0.0-10.255.255.255</srcloc>
					                        <dstloc>US</dstloc>
					                        <app>web-browsing</app>
					                        <vsys>vsys3</vsys>
					                        <from>WINSE</from>
					                        <to>CORP</to>
					                        <inbound_if>ae1.80</inbound_if>
					                        <outbound_if>ae2.80</outbound_if>
					                        <time_received>2015/03/13 15:04:13</time_received>
					                        <sessionid>271773</sessionid>
					                        <repeatcnt>1</repeatcnt>
					                        <sport>54059</sport>
					                        <dport>80</dport>
					                        <natsport>0</natsport>
					                        <natdport>0</natdport>
					                        <flags>0</flags>
					                        <flag-pcap>no</flag-pcap>
					                        <flag-flagged>no</flag-flagged>
					                        <flag-proxy>no</flag-proxy>
					                        <flag-url-denied>no</flag-url-denied>
					                        <flag-nat>no</flag-nat>
					                        <captive-portal>no</captive-portal>
					                        <exported>no</exported>
					                        <transaction>no</transaction>
					                        <pbf-c2s>no</pbf-c2s>
					                        <pbf-s2c>no</pbf-s2c>
					                        <temporary-match>no</temporary-match>
					                        <sym-return>no</sym-return>
					                        <decrypt-mirror>no</decrypt-mirror>
					                        <proto>tcp</proto>
					                        <action>deny</action>
					                        <cpadding>0</cpadding>
					                        <bytes>428</bytes>
					                        <bytes_sent>358</bytes_sent>
					                        <bytes_received>70</bytes_received>
					                        <packets>4</packets>
					                        <start>2015/03/13 15:04:14</start>
					                        <elapsed>0</elapsed>
					                        <category>not-resolved</category>
					                        <padding>0</padding>
					                        <pkts_sent>3</pkts_sent>
					                        <pkts_received>1</pkts_received>
				                        </entry>
			                        </logs>
		                        </log>
	                        </result>
                        </response>";
            #endregion

            using (var memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(Xml)))
            {
                using (var reader = XmlReader.Create(memoryStream))
                {
                    var ser = new XmlSerializer(typeof(GetTrafficLogApiResponse));
                    Assert.IsNotNull((GetTrafficLogApiResponse)ser.Deserialize(reader));
                }
            }
        }
    }
}
