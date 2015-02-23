using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AddingClaimsToRoles.Models
{
    public class ProjectEntities : DbContext
    {
        public ProjectEntities() : base("name=ProjectEntities") { }
        public static ProjectEntities Create()
        {
            return new ProjectEntities();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });

            modelBuilder.Entity<IdentityRole>().HasMany(r => r.Users).WithRequired().HasForeignKey(ur => ur.RoleId);
            modelBuilder.Entity<ApplicationUser>().HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<IdentityClaim>().HasMany(c => c.IdentityRoles).WithMany().Map(
                x => x.MapLeftKey("IdentityClaimID").MapRightKey("IdentityRoleId").ToTable("IdentityClaimRoles")
                );

            //modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id).ToTable("AspNetRoles");
            //modelBuilder.Entity<IdentityUser>().ToTable("AspNetUsers");
            //modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId).ToTable("AspNetUserLogins");
            //modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId }).ToTable("AspNetUserRoles");
            //modelBuilder.Entity<IdentityUserClaim>().ToTable("AspNetUserClaims");
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<IdentityClaim> IdentityClaims { get; set; }
        public DbSet<IdentityRole> IdentityRoles { get; set; }
    }
}