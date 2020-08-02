using Microsoft.Extensions.Configuration;
using System;

namespace NorthWind.Library
{
    public static class Config
    {
        // requires using Microsoft.Extensions.Configuration;
        private static IConfiguration Configuration;

        public static string GetAppSetting(string key)
        {
            string result = string.Empty;
            if (Configuration == null)
            {
                ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
                // Add defaultConfigurationStrings
                configurationBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                Configuration = configurationBuilder.Build();
            }
            result = Configuration[key] != null ? Configuration[key].ToString() : "";
            return result;
        }
    }
}
