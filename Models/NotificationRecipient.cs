using System.ComponentModel.DataAnnotations.Schema;

namespace final_project.Models
{
    public class NotificationRecipient
    {
        public int NotificationRecipientId { get; set; }

        [ForeignKey("Notification")]
        public int NotificationId { get; set; }
        public Notification Notification { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public Student Student { get; set; }
    }

}
