using UserManagementApp.Core.DTOs;
using UserManagementApp.Core.Interfaces;
using UserManagementApp.Core.Models;
using UserManagementApp.Core.Models.Identity;

namespace UserManagementApp.Infrastructure.Services;

public class UserService(IEntityRepository<User> userRepository) : IUserService
{
    public async Task<UserDto?> GetByEmailAsync(string email)
    {
        var user =  await userRepository.FirstOrDefaultAsync(u => u.Email == email);
        return user != null ? new UserDto(user) : null;
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        return (await userRepository.GetAllAsync()).Select(u => new UserDto(u)).OrderByDescending(u => u.LastLogin).ToList();
    }

    public async Task BlockUsersAsync(IEnumerable<int> userIds)
    {
        var users = await userRepository.FindAsync(u => userIds.Contains(u.Id));
        foreach (var user in users)
        {
            user.IsBlocked = true;
            userRepository.Update(user);
        }
        await userRepository.SaveChangesAsync();
    }

    public async Task UnblockUsersAsync(IEnumerable<int> userIds)
    {
        var users = await userRepository.FindAsync(u => userIds.Contains(u.Id));
        foreach (var user in users)
        {
            user.IsBlocked = false;
            userRepository.Update(user);
        }
        await userRepository.SaveChangesAsync();
    }

    public async Task DeleteUsersAsync(IEnumerable<int> userIds)
    {
        var users = await userRepository.FindAsync(u => userIds.Contains(u.Id));
        userRepository.RemoveRange(users);
        await userRepository.SaveChangesAsync();
    }
}