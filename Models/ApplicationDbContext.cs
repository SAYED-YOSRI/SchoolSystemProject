using Microsoft.EntityFrameworkCore;

namespace final_project.Models
{
    public class ApplicationDbContext : DbContext

    {
        public ApplicationDbContext() : base()
        {

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.
                UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=SchoolSystemDB;Trusted_Connection=True;Encrypt=False");
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Admin> Admins { get; set; }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        public DbSet<Exam> Exams { get; set; }
        public DbSet<TimetableEntry> Timetables { get; set; }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationRecipient> NotificationRecipients { get; set; }
        public DbSet<ExamResult> ExamResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Courses -> Teacher relation (No Cascade Delete)
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Teacher)
                .WithMany(t => t.Courses)
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            // Courses -> Department relation (Optional: Cascade Delete or Restrict)
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Department)
                .WithMany(d => d.Courses)
                .HasForeignKey(c => c.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // ExamResult -> Exam relation (No Cascade Delete to prevent multiple cascade paths)
            modelBuilder.Entity<ExamResult>()
                .HasOne(er => er.Exam)
                .WithMany(e => e.ExamResults)
                .HasForeignKey(er => er.ExamId)
                .OnDelete(DeleteBehavior.Restrict);

            // ExamResult -> Enrollment relation (keep cascade if needed)
            modelBuilder.Entity<ExamResult>()
                .HasOne(er => er.Enrollment)
                .WithMany(en => en.ExamResults)
                .HasForeignKey(er => er.EnrollmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Precision for MarksObtained
            modelBuilder.Entity<ExamResult>()
                .Property(er => er.MarksObtained)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ExamResult>()
                .HasKey(er => new { er.ExamId, er.EnrollmentId });
        }




    }
}
