using Microsoft.AspNetCore.Identity;

namespace E_CommerceMVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName}";

        //public string FullName => $"{FirstName} {LastName}";


        // Add admin flag (keep all existing properties)
        public bool IsAdmin { get; set; } = false;


    }
}
