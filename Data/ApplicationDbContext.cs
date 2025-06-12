using Microsoft.EntityFrameworkCore;
using pdf1.Models;

namespace pdf1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ResourceDetail> ResourceDetails { get; set; }

        public DbSet<Education> Educations { get; set; }

        public DbSet<Experience> Experiences { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Certificate> Certificates { get; set; }

        public DbSet<Skill> Skills { get; set; }

        public DbSet<Resourceskill> Resourcesofskill { get; set; }

        public DbSet<Skillgroup> Skillgroups { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //for personal
            modelBuilder.Entity<Education>()
                .HasOne(a => a.res)
                .WithMany(b => b.edu)
                .HasForeignKey(c => c.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Experience>()
                .HasOne(a => a.res)
                .WithMany(b => b.exp)
                .HasForeignKey(c => c.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
                .HasOne(a => a.res)
                .WithMany(b => b.pro)
                .HasForeignKey(c => c.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Certificate>()
                .HasOne(a => a.res)
                .WithMany(b => b.cer)
                .HasForeignKey(c => c.Id)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Skill>()
            //    .HasOne(a => a.res)
            //    .WithMany(b => b.ski)
            //    .HasForeignKey(c => c.Id)
            //    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Resourceskill>()
                .HasOne(a => a.res)
                .WithMany(b => b.rski)
                .HasForeignKey(c => c.ResId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Resourceskill>()
                .HasOne(a => a.sk)
                .WithMany(b => b.rski)
                .HasForeignKey(c => c.SkillId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Skill>()
                .HasOne(a => a.skillgroup)
                .WithMany(b => b.Sk)
                .HasForeignKey(c => c.SkillgroupId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
