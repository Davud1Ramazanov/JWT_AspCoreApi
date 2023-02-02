using System;
using System.Collections.Generic;
using JWT_AspCoreApi.Model;
using Microsoft.EntityFrameworkCore;

namespace JWT_AspCoreApi.Data;

public partial class GadgetstoreDbContext : DbContext
{
    public GadgetstoreDbContext()
    {
    }

    public GadgetstoreDbContext(DbContextOptions<GadgetstoreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Gadget> Gadgets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-3H1AEF60\\SQLEXPRESS;Initial Catalog=gadgetstore_db;Integrated security=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC272192C679");

            entity.ToTable("Category");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.NameGadgets)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NAME_GADGETS");
        });

        modelBuilder.Entity<Gadget>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Gadgets__3214EC278CAB6812");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdCategory).HasColumnName("ID_Category");
            entity.Property(e => e.Name)
                .HasMaxLength(25)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Gadgets)
                .HasForeignKey(d => d.IdCategory)
                .HasConstraintName("FK__Gadgets__ID_Cate__540C7B00");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
