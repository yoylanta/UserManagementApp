using UserManagementApp.Core.DTOs;
using UserManagementApp.Core.Models;
using UserManagementApp.Core.Models.Identity;

namespace UserManagementApp.Core.Interfaces;

public interface IUserService
{
    Task<List<UserDto>> GetAllUsersAsync();
    
    Task BlockUsersAsync(IEnumerable<int> userIds);
    
    Task UnblockUsersAsync(IEnumerable<int> userIds);
    
    Task DeleteUsersAsync(IEnumerable<int> userIds);

    Task<UserDto?> GetByEmailAsync(string email);

}