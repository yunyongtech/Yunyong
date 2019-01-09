using Microsoft.EntityFrameworkCore;

namespace Yunyong.DataExchange.Tests.Models
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserInfo>().ToTable("UserInfo");
        }
    }
}