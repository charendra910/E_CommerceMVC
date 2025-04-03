using System.ComponentModel.DataAnnotations;

namespace E_CommerceMVC.Areas.Dashboard.Models
{
    public class AdminLoginModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Admin Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}