using System.ComponentModel.DataAnnotations;

namespace final_project.Models
{
    public abstract class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string password { get; set; }
        public string phoneNumber { get; set; }
        public string addres {  get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
