using Microsoft.EntityFrameworkCore;

namespace Mutiview_BaseballPark.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // 在這裡加入你的 DbSet 屬性
        // 例如：public DbSet<YourModel> YourModels { get; set; }

        public DbSet<Mutiview_BaseballPark.Models.Stadium> Stadiums { get; set; }
    }
} 