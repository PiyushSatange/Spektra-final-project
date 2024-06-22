using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProjectLab.Models;

public partial class LabContext : DbContext
{
    public LabContext()
    {
    }

    public LabContext(DbContextOptions<LabContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Bucket> Buckets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersBucket> UsersBuckets { get; set; }

    public virtual DbSet<Pr__BucketsByEmailClass> Pr_BucketsByEmailClasses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-6K77UK\\SQLEXPRESS; Initial Catalog=Lab; Integrated Security=True; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("admin");

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Bucket>(entity =>
        {
            entity.HasKey(e => e.BucketId).HasName("PK__Bucket__3214EC077EB9F0B3");

            entity.ToTable("Bucket", tb => tb.HasTrigger("trg_SetDestroyedDateOnDestroyed"));

            entity.Property(e => e.BucketId).HasColumnName("Bucket_Id");
            entity.Property(e => e.BucketName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Bucket_Name");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Created_Date");
            entity.Property(e => e.DestroyedDate)
                .HasColumnType("datetime")
                .HasColumnName("Destroyed_Date");
            entity.Property(e => e.Platform)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.RecordStatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Active")
                .HasColumnName("Record_Status");
            entity.Property(e => e.Region)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ResourceType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Resource_Type");
            entity.Property(e => e.RunningHours).HasColumnName("Running_Hours");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Email).HasName("PK__Users__A9D1053516CA0636");

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Uid)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UsersBucket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users_Bu__3214EC072A308466");

            entity.ToTable("Users_Bucket");

            entity.Property(e => e.BucketId).HasColumnName("Bucket_Id");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("User_Email");

            entity.HasOne(d => d.Bucket).WithMany(p => p.UsersBuckets)
                .HasForeignKey(d => d.BucketId)
                .HasConstraintName("FK_bucket");

            entity.HasOne(d => d.UserEmailNavigation).WithMany(p => p.UsersBuckets)
                .HasForeignKey(d => d.UserEmail)
                .HasConstraintName("FK_users");
        });

        modelBuilder.Entity<Pr__BucketsByEmailClass>().HasNoKey();

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
