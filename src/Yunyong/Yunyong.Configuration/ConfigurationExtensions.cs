using Newtonsoft.Json;

namespace Microsoft.Extensions.Configuration
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