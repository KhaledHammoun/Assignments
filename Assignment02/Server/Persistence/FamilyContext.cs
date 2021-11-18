using Microsoft.EntityFrameworkCore;
using Model;

namespace Server.Persistence
{
    public class FamilyContext : DbContext
    {
        public DbSet<Adult> Adults { set; get; }
        public DbSet<User> Users { set; get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(
                @"Data Source = C:\C#\3. Semester Three\Assignments\Assignment02\Server\FamilyContext.db");
        }
    }
}