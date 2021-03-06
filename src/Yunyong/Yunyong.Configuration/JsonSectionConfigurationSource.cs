﻿using System.Collections.Generic;

namespace Microsoft.Extensions.Configuration
{
    public class JsonSectionConfigurationSource : IConfigurationSource
    {
        private readonly Dictionary<string, object> data = new Dictionary<string, object>();

        public object this[string key]
        {
            get => data.ContainsKey(key) ? data[key] : string.Empty;
            set
            {
                if (key == null)
                {
                    return;
                }

                data[key] = value;
            }
        }

        public IEnumerable<string> Keys { get => new List<string>(data.Keys); }
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            var provider = new JsonSectionConfigurationProvider(this);

            return provider;
        }
    }
}