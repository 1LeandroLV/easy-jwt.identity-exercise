namespace easy_jwt.identity_exercise.Models
//denna klass anväds för att begränsa status till gilitiga värden:
{
    public enum EnrollmentStatus
    {
        // student har ansökt, vänta på godkännande
        pending,
        //student ska få tillgång till kursen enligt reglerna 
        Approved
    }
}

