namespace PANOSPsTest
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Management.Automation;

    using PANOS;

    public class PsTestRunner<T> where T : FirewallObject
    {
        private readonly string connection;
        
        public PsTestRunner()
        {
            connection = string.Format("$connection = New-PANOSConnection -HostName '{0}' -Vsys '{1}' -AccessToken (ConvertTo-SecureString '{2}' -AsPlainText -Force) -StoreInSession | Out-Null",
                    ConfigurationManager.AppSettings["FirewallHostName"],
                    ConfigurationManager.AppSettings["Vsys"],
                    ConfigurationManager.AppSettings["FirewallAccessToken"]);
        }

        public List<T> ExecuteQuery(string script)
        {
            return PowerShell.Create().AddScript(string.Format("{0};{1}", connection, script)).Invoke<T>().ToList();
        }
    }
}
