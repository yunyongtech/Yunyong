using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microsoft.Extensions.Configuration
{
    public class JsonScriptConfigurationProvider : ConfigurationProvider
    {
        private readonly Dictionary<string, object> data;

        public JsonScriptConfigurationProvider(Dictionary<string, object> data)
        {
            this.data = data;
        }


        public override bool TryGet(string key, out string value)
        {
            if (data.TryGetValue(key, out var val))
            {
                value = JsonConvert.SerializeObject(val);
                return true;
            }

            return base.TryGet(key, out value);
        }
    }
}