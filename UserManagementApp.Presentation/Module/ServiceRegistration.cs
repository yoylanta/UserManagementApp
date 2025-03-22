using Microsoft.AspNetCore.Identity;
using UserManagementApp.Core.Interfaces;
using UserManagementApp.Core.Models.Identity;
using UserManagementApp.Infrastructure.Repositories;
using UserManagementApp.Infrastructure.Services;

namespace UserManagementApp.Presentation.Module;

public static class ServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IEntityRepository<User>, EntityRepository<User>>();
        services.AddScoped<IUserService, UserService>();
        services.AddTransient<IUserValidator<User>, CustomUserValidator>();
    }
}