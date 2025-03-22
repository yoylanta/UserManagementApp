using UserManagementApp.Core.DTOs;
using UserManagementApp.Core.Interfaces;
using UserManagementApp.Core.Models.Identity;

namespace UserManagementApp.Presentation.Pages;

public class IndexModel(ILogger<IndexModel> logger, IUserService userService) : BasePageModel(userService)
{
    public List<UserDto> Users { get; set; }
    
    public async Task OnGetAsync()
    {
        await RedirectIfNotAuthenticated();
        Users = await userService.GetAllUsersAsync();
    }
}