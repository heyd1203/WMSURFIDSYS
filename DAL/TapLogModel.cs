namespace DAL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TapLogModel : DbContext
    {
        public TapLogModel()
            : base("name=TapLogModel")
        {
        }

        public virtual DbSet<College> Colleges { get; set; }
        public virtual DbSet<Cours> Courses { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<TapLog> TapLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<College>()
                .Property(e => e.CollegeName)
                .IsUnicode(false);

            modelBuilder.Entity<College>()
                .Property(e => e.CollegeAbbv)
                .IsUnicode(false);

            modelBuilder.Entity<College>()
                .HasMany(e => e.Students)
                .WithRequired(e => e.College)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Cours>()
                .Property(e => e.CourseName)
                .IsUnicode(false);

            modelBuilder.Entity<Cours>()
                .Property(e => e.CourseAbbv)
                .IsUnicode(false);

            modelBuilder.Entity<Cours>()
                .HasMany(e => e.Students)
                .WithRequired(e => e.Cours)
                .HasForeignKey(e => e.CourseID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasMany(e => e.TapLogs)
                .WithRequired(e => e.Student)
                .WillCascadeOnDelete(false);
        }
    }
}
