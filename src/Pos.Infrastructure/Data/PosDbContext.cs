using Microsoft.EntityFrameworkCore;
using Pos.Domain.Entities;

namespace Pos.Infrastructure.Data;

public class PosDbContext : DbContext
{
    public PosDbContext(DbContextOptions<PosDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<SaleItem> SaleItems => Set<SaleItem>();
    public DbSet<CashFlow> CashFlows => Set<CashFlow>();
    public DbSet<CashBox> CashBoxes => Set<CashBox>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserPermission> UserPermissions => Set<UserPermission>();
    public DbSet<UserActivityLog> UserActivityLogs => Set<UserActivityLog>();
    public DbSet<Return> Returns => Set<Return>();
    public DbSet<ReturnItem> ReturnItems => Set<ReturnItem>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Permission>(builder =>
        {
            builder.HasIndex(p => p.Code)
                .IsUnique();
        });

        modelBuilder.Entity<UserPermission>(builder =>
        {
            builder.HasKey(up => new { up.UserId, up.PermissionId });

            builder.HasOne(up => up.User)
                .WithMany(u => u.UserPermissions)
                .HasForeignKey(up => up.UserId);

            builder.HasOne(up => up.Permission)
                .WithMany(p => p.UserPermissions)
                .HasForeignKey(up => up.PermissionId);
        });

        modelBuilder.Entity<UserActivityLog>(builder =>
        {
            builder.Property(a => a.Description)
                .HasMaxLength(500);

            builder.HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId);

            builder.HasOne(a => a.CashBox)
                .WithMany()
                .HasForeignKey(a => a.CashBoxId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(a => a.Product)
                .WithMany()
                .HasForeignKey(a => a.ProductId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(a => a.CashBoxId);
            builder.HasIndex(a => a.UserId);
            builder.HasIndex(a => a.CreatedAt);
        });

        modelBuilder.Entity<Return>(builder =>
        {
            builder.HasIndex(r => r.SaleId);

            builder.HasOne(r => r.Sale)
                .WithMany()
                .HasForeignKey(r => r.SaleId);

            builder.HasOne(r => r.User)
                .WithMany(u => u.Returns)
                .HasForeignKey(r => r.UserId);
        });

        modelBuilder.Entity<ReturnItem>(builder =>
        {
            builder.HasOne(i => i.Return)
                .WithMany(r => r.Items)
                .HasForeignKey(i => i.ReturnId);

            builder.HasOne(i => i.SaleItem)
                .WithMany()
                .HasForeignKey(i => i.SaleItemId);

            builder.HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId);
        });

        modelBuilder.Entity<Product>(builder =>
        {
            builder.HasIndex(p => p.Sku).IsUnique();
            builder.Property(p => p.UpdatedAt).IsConcurrencyToken();

            builder.HasOne(p => p.CreatedBy)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.CreatedById);

            builder.HasOne(p => p.UpdatedBy)
                .WithMany()
                .HasForeignKey(p => p.UpdatedById);

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);
        });

        modelBuilder.Entity<Sale>(builder =>
        {
            builder.Property(s => s.UpdatedAt).IsConcurrencyToken();

            builder.HasOne(s => s.User)
                .WithMany(u => u.Sales)
                .HasForeignKey(s => s.UserId);
            
            builder.HasOne(c => c.CashBox)
                .WithMany(s => s.Sales)
                .HasForeignKey(s => s.CashBoxId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<SaleItem>(builder =>
        {
            builder.HasOne(i => i.Sale)
                .WithMany(s => s.Items)
                .HasForeignKey(i => i.SaleId);

            builder.HasOne(i => i.Product)
                .WithMany(p => p.SaleItems)
                .HasForeignKey(i => i.ProductId);

            builder.HasIndex(i => new { i.SaleId, i.ProductId });
        });

        modelBuilder.Entity<CashFlow>(builder =>
        {
            builder.HasOne(c => c.User)
                .WithMany(u => u.CashFlows)
                .HasForeignKey(c => c.UserId);
            builder.HasOne(c => c.CashBox)
                .WithMany(c => c.CashFlows)
                .HasForeignKey(c => c.CashBoxId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<CashBox>(builder =>
        {
            builder.Property(c => c.UpdatedAt).IsConcurrencyToken();

            builder.HasOne(c => c.User)
                .WithMany(u => u.CashBoxes)
                .HasForeignKey(c => c.UserId);
            
            builder.HasMany(s => s.Sales)
                .WithOne(c => c.CashBox)
                .HasForeignKey(c => c.CashBoxId);
                
        });

        modelBuilder.Entity<User>(builder =>
        {
            builder.Property(u => u.IsOwner).HasDefaultValue(false);
            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.NormaliceName).IsUnique();
            builder.HasIndex(u => u.IsOwner)
                .IsUnique()
                .HasFilter("\"IsOwner\" = TRUE");
        });

        modelBuilder.Entity<Category>(builder =>
        {
            builder.HasIndex(c => c.Name).IsUnique();
        });
    }
}
