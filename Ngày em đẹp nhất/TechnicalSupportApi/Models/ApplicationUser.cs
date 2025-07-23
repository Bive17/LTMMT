using Microsoft.AspNetCore.Identity;
namespace TechnicalSupportApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public string? Expertise { get; set; } // For technicians (e.g., "Hardware", "Software")
    }
}
