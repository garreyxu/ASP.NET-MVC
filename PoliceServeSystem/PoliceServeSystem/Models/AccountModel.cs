using System.ComponentModel.DataAnnotations;

namespace PoliceServeSystem.Models
{
    public class AccountModel
    {
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Module")]
        public string ModuleType { get; set; }

        [Required]
        public string NotificationType { get; set; }
    }
}