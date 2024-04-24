using System;
using System.Collections.Generic;
using Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace Data.DataContext;

public partial class AssignmentContext : DbContext
{
    public AssignmentContext()
    {
    }

    public AssignmentContext(DbContextOptions<AssignmentContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("User ID =postgres;Password=178@tatva;Server=localhost;Port=5432;Database=Assignment;Integrated Security=true;Pooling=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Course_pkey");

            entity.ToTable("Course");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.Name).HasMaxLength(500);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Student_pkey");

            entity.ToTable("Student");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.Course).HasMaxLength(500);
            entity.Property(e => e.Email).HasMaxLength(500);
            entity.Property(e => e.FirstName).HasMaxLength(500);
            entity.Property(e => e.Gender).HasMaxLength(128);
            entity.Property(e => e.Grade).HasMaxLength(128);
            entity.Property(e => e.LastName).HasMaxLength(500);

            entity.HasOne(d => d.CourseNavigation).WithMany(p => p.Students)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("Student_CourseId_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
