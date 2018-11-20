using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Yunyong.Configuration
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