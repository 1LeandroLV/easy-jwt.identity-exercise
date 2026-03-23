using easy_jwt.identity_exercise.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

// här har jag sagt: vilka tabller som ska finnas, 
// hur relationerna ser ut, vilka regler som gäller

namespace easy_jwt.identity_exercise.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        // används för att konfiguera databasen; hur realtioner fungera,
        // vilka foreign keys som finns, unika regler, extra konfiguration
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // kör först identitys egen databas konfiguration
            // glömmer man den kan identity tabller sluta fungera
            base.OnModelCreating(builder);

            builder.Entity<Course>()
                .HasOne(c => c.Teacher)
                .WithMany()
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany()
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Enrollment>()
                .HasIndex(e => new { e.CourseId, e.StudentId })
                .IsUnique();
        }
    }
}