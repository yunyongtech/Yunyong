using System;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Yunyong.Cache;
using Yunyong.Cache.Abstractions;
using Yunyong.Cache.Register;

namespace Tests
{
    public class Tests
    {
        T GetService<T>()
        {
            return Scope.ServiceProvider.GetService<T>();
        }
        [SetUp]
        public void Setup()
        {
            IServiceCollection services = new ServiceCollection();
            services.RegisterRedisCache(new CacheServiceConfig()
            {
                ConnectionString = "localhost",
                Port = 6379,
                Password = "RainbowCache2019",
            });

            var Provider = services.BuildServiceProvider();
            this.Scope = Provider.CreateScope();
        }

        public IServiceScope Scope { get; set; }

        [Test]
        public void Test1()
        {
            var service = GetService<ICacheService<MyData>>();

            for (int i = 0; i < 10; i++)
            {
                var item = new MyData()
                {
                    Id = i,
                    Name = $"TestName_{i:000}"
                };

                service.Set($"{item.Id:000}", item);
            }

            for (int i = 0; i < 10; i++)
            {
                var id = $"{i:000}";
                var item = service.GetOrDefault<MyData>(id);
                Assert.NotNull(item);
            }

            {
                var keys = service.GetKeys();
                var items = service.GetAll<MyData>();
                Assert.AreEqual(10, items.Count);

                for (int i = 0; i < 10; i++)
                {
                    Assert.IsTrue(service.KeyExists($"{i:000}"));
                }

            }
        }
    }

    public class MyData
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}