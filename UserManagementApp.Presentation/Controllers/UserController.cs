using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagementApp.Core.Interfaces;
using UserManagementApp.Core.Models.Identity;

namespace UserManagementApp.Presentation.Controllers;

[Authorize]
[Route("api/user")] 
public class UserController(UserManager<User> userManager, IUserService userService) : Controller
{
    [HttpGet]
    [IgnoreAntiforgeryToken]
    [Route("index")] 
    public async Task<IActionResult> Index()
    {
        var currentUser = await userManager.GetUserAsync(User);
        if (currentUser == null || currentUser.IsBlocked)
        {
            return Unauthorized();
        }

        var users = await userService.GetAllUsersAsync();
        return View(users);
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    [Route("block")] 
    public async Task<IActionResult> BlockUsers([FromBody] List<int> userIds)
    {
        var currentUser = await userManager.GetUserAsync(User);
        if (currentUser == null || currentUser.IsBlocked)
        {
            return Unauthorized();
        }

        await userService.BlockUsersAsync(userIds);
        return Ok();
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    [Route("unblock")] 
    public async Task<IActionResult> UnblockUsers([FromBody] List<int> userIds)
    {
        var currentUser = await userManager.GetUserAsync(User);
        if (currentUser == null || currentUser.IsBlocked)
        {
            return Unauthorized();
        }

        await userService.UnblockUsersAsync(userIds);
        return Ok();
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    [Route("delete")] 
    public async Task<IActionResult> DeleteUsers([FromBody] List<int> userIds)
    {
        var currentUser = await userManager.GetUserAsync(User);
        if (currentUser == null || currentUser.IsBlocked)
        {
            return Unauthorized();
        }

        await userService.DeleteUsersAsync(userIds);
        return Ok();
    }
}