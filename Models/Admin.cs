namespace final_project.Models
{
    public class Admin : User
    {
        public int AdminId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public List<Notification> NotificationsSent { get; set; }
    }

}
