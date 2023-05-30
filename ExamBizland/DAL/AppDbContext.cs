using ExamBizland.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExamBizland.DAL
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options)
        {

        }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<BizlandService> Services { get; set; }
        public DbSet<Setting> Settings { get; set; }
    }
}
