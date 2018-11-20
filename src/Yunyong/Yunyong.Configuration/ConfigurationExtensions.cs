using Newtonsoft.Json;

namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationExtensions
    {
        public static T Get<T>(this IConfiguration config, string name)
        {
            return config.GetSection(name).Get<T>();
        }
    }
}