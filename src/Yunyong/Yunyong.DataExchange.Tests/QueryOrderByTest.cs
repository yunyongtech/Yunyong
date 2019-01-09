using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;
using Yunyong.Core;
using Yunyong.DataExchange.Tests.Models;

namespace Yunyong.DataExchange.Tests
{
    public class QueryOrderByTest : TestBase
    {
        [SetUp]
        public void Setup()
        {
            var servcies = new ServiceCollection();
            servcies.AddDbContext<TestDbContext>(opt => { opt.UseMySql("Server=localhost; Database=YunyongDataExchangeTestDB_20190109; Uid=SkyUser; Pwd=Sky@4321;SslMode=none;"); });

            var provider = servcies.BuildServiceProvider();

            var scope = provider.CreateScope();
            var context = scope.ServiceProvider.GetService<TestDbContext>();
            context.Database.EnsureCreated();


            if (Conn.CountAsync<UserInfo>(a => true).Result == 0)
            {
                var rand = new Random();
                for (int i = 0; i < 100; i++)
                {
                    var tmp = EntityFactory.Create<UserInfo>();
                    tmp.Name = $"TestUser{rand.Next(1000, 9999):0000}";
                    Conn.CreateAsync(tmp).GetAwaiter().GetResult();
                }
            }
        }

        [Test]
        public async Task Test1()
        {
            {
                Conn.OpenDebug();
                var result = await Conn.ListAsync<UserInfo>(a => a.Name.Contains("2"), new[] { new OrderBy() { Field = "Name", Desc = true }, new OrderBy() { Field = "CreatedOn" }, });
            }
            Assert.Pass();
        }
        [Test]
        public async Task TestTopOrderBy()
        {
            {
                Conn.OpenDebug();
                var result = await Conn.TopAsync<UserInfo>(5, a => a.Name.Contains("2"), new[] { new OrderBy() { Field = "Name", Desc = true }, new OrderBy() { Field = "CreatedOn" }, });
            }
            Assert.Pass();
        }
    }
}