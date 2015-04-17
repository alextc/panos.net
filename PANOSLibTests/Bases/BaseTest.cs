namespace PANOSLibTest
{
    using System.Configuration;

    using PANOS;

    public class BaseTest
    {
        protected static Connection Connection
        {
            get
            {
                return new Connection(
                    ConfigurationManager.AppSettings["FirewallHostName"],
                    SecureStringUtils.ConvertToSecureString((ConfigurationManager.AppSettings["FirewallAccessToken"])),
                    ConfigurationManager.AppSettings["Vsys"]);
            }
        }
    }
}
