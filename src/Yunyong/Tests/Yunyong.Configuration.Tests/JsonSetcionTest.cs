using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Tests
{
    public class JsonSetcionTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var builder = new ConfigurationBuilder();
            builder.Add(new JsonSectionConfigurationSource()
            {
                ["ServiceConfig"] = new ServiceConfig()
                {
                    Name = "TestService",
                },
                ["Value"] = 12
            });
            var configuration = builder.Build();
            var config = configuration.GetSection("ServiceConfig").Get<ServiceConfig>();
            //var config = configuration.Get<ServiceConfig>("ServiceConfig");

            var val = configuration.Get<int>("Value");

            Assert.AreEqual("TestService", config.Name);
        }

    }
}