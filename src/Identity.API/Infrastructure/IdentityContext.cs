using Identity.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Infrastructure;

public class IdentityContext : DbContext, IDisposable
{
    public IdentityContext(DbContextOptions options) : base(options) 
    { 
    
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .Property(x => x.Username).IsRequired();
        modelBuilder.Entity<User>()
            .Property(x => x.Password).IsRequired();

        modelBuilder.Entity<Role>()
            .Property(x => x.Name).IsRequired();
        
        modelBuilder.Entity<Permission>()
            .Property(x => x.Name).IsRequired();

        modelBuilder.Entity<UserRole>()
            .HasKey(x => new { x.UserId, x.RoleId });
        modelBuilder.Entity<UserRole>()
            .HasOne(x => x.Role)
            .WithMany(x => x.UserRoles)
            .HasForeignKey(x => x.RoleId);
        modelBuilder.Entity<UserRole>()
            .HasOne(x => x.User)
            .WithMany(x => x.UserRoles)
            .HasForeignKey(x => x.UserId);
        
        modelBuilder.Entity<RolePermission>()
            .HasKey(x => new { x.PermissionId, x.RoleId });
        modelBuilder.Entity<RolePermission>()
            .HasOne(x => x.Role)
            .WithMany(x => x.RolePermissions)
            .HasForeignKey(x => x.RoleId);
        modelBuilder.Entity<RolePermission>()
            .HasOne(x => x.Permission)
            .WithMany(x => x.RolePermissions)
            .HasForeignKey(x => x.PermissionId);
    }
}
