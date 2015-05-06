namespace PANOSPsTest
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Management.Automation;
    using System.Management.Automation.Runspaces;

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

        public void ExecuteCommand(string script)
        {
            using (var powerShellInstance = PowerShell.Create())
            {
                powerShellInstance.AddScript(string.Format("{0};{1}", connection, script)).Invoke<T>();
                if (powerShellInstance.Streams.Error.Count > 0)
                {
                    throw new Exception(powerShellInstance.Streams.Error[0].Exception.Message);
                }
            }
        }

        // Example: Remove-PANOSAddress -Name TestAddress -PassThru 
        // In this case the object being passed through is String and not AddressObject, so supply TPassThru explicitely
        public TPassThru ExecuteCommandWithPasThru<TPassThru>(string script)
        {
            return PowerShell.Create().AddScript(string.Format("{0};{1}", connection, script)).Invoke<TPassThru>().Single();
        }

        public T ExecuteCommandWithPasThru(string script)
        {
            return PowerShell.Create().AddScript(string.Format("{0};{1}", connection, script)).Invoke<T>().Single();
        }
    }
}
