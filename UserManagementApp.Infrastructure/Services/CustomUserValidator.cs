using Microsoft.AspNetCore.Identity;
using UserManagementApp.Core.Models.Identity;

namespace UserManagementApp.Infrastructure.Services;

public class CustomUserValidator : IUserValidator<User>
{
    public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
    {
        return Task.FromResult(IdentityResult.Success);  // No validation errors
    }
}