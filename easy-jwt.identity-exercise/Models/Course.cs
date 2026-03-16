namespace easy_jwt.identity_exercise.Models
{
    public class Course
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty; //anväd för att unvika null-problem.

       //detta spara id:t för den teacher som äger kursen 
        public string TeacherID { get; set; } = string.Empty;

        public bool IsPublished { get; set; } //om kursen är publicerad eller inte 

        //kursen kan kopllas till en teacher-anvädare.
        public AppUser Teacher { get; set; } = null!;
        public ICollection<Enrollments> Enrollments { get; set; } = new List<Enrollments>();
    }
}
