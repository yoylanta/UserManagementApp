using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagementApp.Core.Interfaces;

namespace UserManagementApp.Presentation.Pages;

public class BasePageModel(IUserService userService) : PageModel
{
    protected async Task RedirectIfNotAuthenticated()
    {
        if (User.Identity?.IsAuthenticated ?? false)
        {
            var email = User.Identity.Name;
            var user = await userService.GetByEmailAsync(email);

            if (user == null || user.IsBlocked)
            {
                Response.Redirect("/Account/Login");
            }
        }
        else
        {
            Response.Redirect("/Account/Login");
        }
    }
}