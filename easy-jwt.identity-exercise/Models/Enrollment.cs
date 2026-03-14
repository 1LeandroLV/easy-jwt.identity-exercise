namespace easy_jwt.identity_exercise.Models
    //denna klass anväds för att begränsa status till gilitiga värden:
{
    public enum Enrollment
    {
        // student har ansökt, vänta på godkännande
        peding,
        //student ska få tillgång till kursen enligt reglerna 
        Approved
    }
}
