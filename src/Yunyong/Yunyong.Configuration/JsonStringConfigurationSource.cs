namespace Microsoft.Extensions.Configuration
{
    public class JsonStringConfigurationSource : IConfigurationSource
    {
        internal string JsonString { get; }

        public JsonStringConfigurationSource(string jsonString)
        {
            JsonString = jsonString;
        }
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            var provider = new JsonStringConfigurationProvider(this);

            return provider;
        }
    }
}