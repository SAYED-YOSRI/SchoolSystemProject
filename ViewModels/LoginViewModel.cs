using System.ComponentModel.DataAnnotations;

namespace final_project.ViewModels
{
    // Models/ViewModels/LoginViewModel.cs
    public class LoginViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "Username must be between 3 and 100 characters.", MinimumLength = 3)]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

}
