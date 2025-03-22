using UserManagementApp.Core.Models.Identity;

namespace UserManagementApp.Core.DTOs;

public class UserDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }
    public DateTime? LastLogin { get; set; }
    public DateTime RegistrationTime { get; init; }
    public bool IsBlocked { get; set; }

    public UserDto(User user)
    {
        Id = user.Id;
        Name = user.Name;
        Email = user.Email!;
        LastLogin = user.LastLogin;
        RegistrationTime = user.RegistrationTime;
        IsBlocked = user.IsBlocked;
    }
}