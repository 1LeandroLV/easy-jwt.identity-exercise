namespace easy_jwt.identity_exercise.Models
{
    public class Enrollments 
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string StudentId { get; set; } = string.Empty;

        public EnrollmentStatus Status { get; set; }

        public Course Course { get; set; } = null;

        public AppUser Student { get; set; } = null!;

    }
}
