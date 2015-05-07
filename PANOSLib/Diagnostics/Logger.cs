namespace PANOS
{
    using System.IO;
    using log4net.Config;

    public static class Logger
    {
        private const string Log4NetConfigFile = "log4netConfig.xml";
        private static bool isConfigured;

        public static void Configure()
        {
            if (!isConfigured)
            {
                var log4NetConfigPath = Path.Combine(FileUtils.GetExecutingAssemblyPath(), Log4NetConfigFile);
                // Should I check if file exists? File.Exists(log4NetConfigPath); and throw an exception if it does not or silently continue?
                XmlConfigurator.Configure(new FileInfo(log4NetConfigPath));
                isConfigured = true;
            }
        }
    }
}
