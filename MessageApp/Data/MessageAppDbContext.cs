using MessageApp.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MessageApp.Data
{
    public class MessageAppDbContext : IdentityDbContext<ChatUsers, IdentityRole, string>
    {

        public MessageAppDbContext(DbContextOptions options) : base(options) { }

        public MessageAppDbContext()
        {
        }

        public DbSet<ChatUsers> ChatUsers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=MessageAppDb;Trusted_Connection=true");
            }
            base.OnConfiguring(optionsBuilder);
        }
    }
}
