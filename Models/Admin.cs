namespace final_project.Models
{
    public class Admin : User
    {
        public List<Notification> NotificationsSent { get; set; }
    }

}
