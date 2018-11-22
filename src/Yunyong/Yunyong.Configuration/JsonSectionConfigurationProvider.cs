namespace Microsoft.Extensions.Configuration
{
    public class JsonSectionConfigurationProvider : ConfigurationProvider
    {
        private JsonSectionConfigurationSource Source { get; }
        //private readonly Dictionary<string, object> data;

        public JsonSectionConfigurationProvider(JsonSectionConfigurationSource source)
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