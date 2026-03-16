using easy_jwt.identity_exercise.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace easy_jwt.identity_exercise.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }    
          public DbSet<Course> Courses { get; set; }
          public DbSet<Enrollments> Enrollments { get; set; }


        // används för att konfiguera databasen; hur realtioner fungera,
        //vilka foreign keys som finns, unika regler, extra konfiguration

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //kör fört identitys egen dabas konfiguration
            //glömmer man den kan identity tabller sluta fungera 
            base.OnModelCreating(builder);

            builder.Entity<Course>() //konfiguera modellen Course
            .HasOne(C => c.Teacher) //en course har en teacher
            .withMany(); //en teacher kan ha många courses 

            builder.Entity<Enrollments>() //konfiguera modelln Enrollment
            .HasOne(e => e.Course)//en course har en course
            .WithMany(c => c.Enrollments); //Course kan ha många Enrollments

            builder.Entity<Enrollments>()
            .HasOne(e => e.Student)
            .WithMany();

            builder.Entity<Enrollments>()
            .HasIndex(e => new { e.CourseId, e.StudentId })
            .IsUnique();
        }
    }
}
