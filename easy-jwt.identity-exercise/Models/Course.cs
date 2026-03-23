namespace easy_jwt.identity_exercise.Models
{
    public class Course
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string TeacherId { get; set; } = string.Empty;

        public bool IsPublished { get; set; }

        public AppUser Teacher { get; set; } = null!;

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}