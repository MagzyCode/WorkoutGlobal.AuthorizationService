using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutGlobal.AuthorizationServiceApi.IntegrationTests.Configuration
{
    public static class ConfigurationAccessor
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
