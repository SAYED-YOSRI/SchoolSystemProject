using System.ComponentModel.DataAnnotations.Schema;

namespace final_project.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; } = DateTime.Now;

        [ForeignKey("Admin")]
        public int? AdminId { get; set; }
        public Admin Admin { get; set; }

        public List<NotificationRecipient> Recipients { get; set; }
    }

}
