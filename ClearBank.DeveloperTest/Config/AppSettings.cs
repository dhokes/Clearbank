using System.Configuration;

namespace ClearBank.DeveloperTest.Config
{
    public class AppSettings : IAppSettings
    {
        public string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}