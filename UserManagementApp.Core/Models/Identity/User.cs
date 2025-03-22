using Microsoft.AspNetCore.Identity;

namespace UserManagementApp.Core.Models.Identity
{
    public class User : IdentityUser<int>
    {
        public string Name { get; init; }
        public DateTime? LastLogin { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime RegistrationTime { get; init; }
    }
}