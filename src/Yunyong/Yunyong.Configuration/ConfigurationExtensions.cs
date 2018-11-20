using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Yunyong.Configuration
{
    public static class ConfigurationExtensions
    {
        public static T Get<T>(this IConfiguration config, string name)
        {
            var str = config.GetValue<string>(name);
            if (string.IsNullOrWhiteSpace(str))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(str);
        }
    }
}