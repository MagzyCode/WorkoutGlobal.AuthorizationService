using Microsoft.Extensions.Configuration;
using System.IO;

namespace WorkoutGlobal.AuthorizationServiceApi.UnitTests.Configuration
{
    internal class ConfigurationAccessor
    {
        public static IConfiguration GetTestConfiguration(string settingFilePath = "appsettings.json")
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(settingFilePath, optional: false, reloadOnChange: true)
                .Build();

            return configuration;
        }
    }
}
