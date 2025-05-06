namespace final_project.Models
{
    public class Student : User
    {
        public int StudentId { get; set; }
        

        public List<Enrollment> Enrollments { get; set; }
        public List<NotificationRecipient> Notifications { get; set; }
    }
}
