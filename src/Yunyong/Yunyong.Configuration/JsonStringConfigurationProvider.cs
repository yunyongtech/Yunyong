namespace Microsoft.Extensions.Configuration
{
    public class JsonStringConfigurationProvider : ConfigurationProvider
    {
        private JsonStringConfigurationSource Source { get; }

        public JsonStringConfigurationProvider(JsonStringConfigurationSource source)
        {
            Source = source;
        }

        public override void Load()
        {
            Data = new JsonDataConfigurationParser().Parse(Source);
            base.Load();
        }
    }
}