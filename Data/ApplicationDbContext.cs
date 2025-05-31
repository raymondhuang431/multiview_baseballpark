using Microsoft.EntityFrameworkCore;
using Mutiview_BaseballPark.Models;

namespace Mutiview_BaseballPark.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // 移除了 DbSet<Image>，因為圖片操作主要使用 Dapper
        public DbSet<Stadium> Stadiums { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 移除了 Image 相關的所有 EF Core 映射配置
            // 因為圖片操作使用 Dapper

            // 設定 Stadium 表名和欄位名 (保留 Stadium 的映射配置)
            modelBuilder.Entity<Stadium>().ToTable("stadiums");
            modelBuilder.Entity<Stadium>().Property(s => s.StadiumId).HasColumnName("stadium_id");
            // 為 main_image_url 欄位添加配置，明確允許 NULL
            modelBuilder.Entity<Stadium>().Property(s => s.MainImageUrlFilename).HasColumnName("main_image_url").IsRequired(false);
            // 你可以在這裡為其他 Stadium 屬性添加 HasColumnName 設定，如果資料庫欄位名稱不同
        }
    }
} 