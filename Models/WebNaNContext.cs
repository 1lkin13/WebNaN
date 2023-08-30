using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebNaN.Models
{
    public partial class WebNaNContext : DbContext
    {
        public WebNaNContext()
        {
        }

        public WebNaNContext(DbContextOptions<WebNaNContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Photo> Photos { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=ZEO\\SQLExpress;Database=WebNaN;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.CategoryName).HasMaxLength(50);

                entity.Property(e => e.CategoryPic).HasMaxLength(150);
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.Property(e => e.PhotoUrl)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.PhotoProduct)
                    .WithMany(p => p.Photos)
                    .HasForeignKey(d => d.PhotoProductId)
                    .HasConstraintName("FK__Photos__PhotoPro__02FC7413");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ProductInfo)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_NaN");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.Property(e => e.StatusName).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.UserName, "UQ__Users__C9F28456C1352111")
                    .IsUnique();

                entity.Property(e => e.UserName).HasMaxLength(25);

                entity.Property(e => e.UserPassword).HasMaxLength(150);

                entity.HasOne(d => d.UserStatus)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserStatusId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Users__UserStatu__74AE54BC");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
