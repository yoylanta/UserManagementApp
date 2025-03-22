using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagementApp.Core.Models.Identity;

namespace UserManagementApp.Presentation.Pages.Account
{
    public class LoginModel(SignInManager<User> signInManager, UserManager<User> userManager) : PageModel
    {
        [BindProperty]
        public required InputModel Input { get; set; }

        public string? ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            public bool RememberMe { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var currentUser = await userManager.GetUserAsync(User);
                    if (!currentUser!.IsBlocked)
                    {
                        currentUser!.LastLogin = DateTime.UtcNow;
                        await userManager.UpdateAsync(currentUser);
                        return RedirectToPage("/Index");
                    }
                    ModelState.AddModelError(string.Empty, $"{currentUser.Name}, you are blocked!");
                    return Page();
                }
                
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            return Page();
        }
    }
}
