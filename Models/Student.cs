namespace final_project.Models
{
    public class Student : User
    {
        public int StudentId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public List<Enrollment> Enrollments { get; set; }
        public List<NotificationRecipient> Notifications { get; set; }
    }
}
