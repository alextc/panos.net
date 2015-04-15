namespace PANOSLibTest
{
    using System.Configuration;

    using PANOS;

    public class BaseTest
    {
        protected static ConnectionProperties ConnectionProperties
        {
            get
            {
                return new ConnectionProperties(
                    ConfigurationManager.AppSettings["FirewallHostName"],
                    SecureStringUtils.ConvertToSecureString((ConfigurationManager.AppSettings["FirewallAccessToken"])),
                    ConfigurationManager.AppSettings["Vsys"]);
            }
        }
    }
}
