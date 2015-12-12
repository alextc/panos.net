namespace PANOSPsTest
{
    using System;
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
            connection =
                $"$connection = New-PANOSConnection -HostName '{ConfigurationManager.AppSettings["FirewallHostName"]}' -Vsys '{ConfigurationManager.AppSettings["Vsys"]}' -AccessToken (ConvertTo-SecureString '{ConfigurationManager.AppSettings["FirewallAccessToken"]}' -AsPlainText -Force) -StoreInSession | Out-Null";
        }

        public List<T> ExecuteQuery(string script)
        {
            return PowerShell.Create().AddScript($"{this.connection};{script}").Invoke<T>().ToList();    
        }

        public void ExecuteCommand(string script)
        {
            using (var powerShellInstance = PowerShell.Create())
            {
                powerShellInstance.AddScript($"{connection};{script}").Invoke<T>();
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
            return PowerShell.Create().AddScript($"{connection};{script}").Invoke<TPassThru>().Single();
        }

        public T ExecuteCommandWithPasThru(string script)
        {
            return PowerShell.Create().AddScript($"{connection};{script}").Invoke<T>().Single();
        }
    }
}
