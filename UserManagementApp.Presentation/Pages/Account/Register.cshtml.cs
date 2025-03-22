using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using UserManagementApp.Core.Interfaces;
using UserManagementApp.Core.Models.Identity;

namespace UserManagementApp.Presentation.Pages.Account
{
    public class RegisterModel(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IEntityRepository<User> userRepository)
        : PageModel
    {
        [BindProperty]
        public required InputModel Input { get; set; }

        public string? ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
            [Display(Name = "Full Name")]
            public string Name { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
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
                var user = new User
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    Name = Input.Name,
                    RegistrationTime = DateTime.UtcNow
                };

                try
                {
                    var result = await userManager.CreateAsync(user, Input.Password);
                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToPage("/Account/Login");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException is PostgresException postgresEx && postgresEx.SqlState == "23505")
                    {
                        if (postgresEx.ConstraintName == "IX_AspNetUsers_Email")
                        {
                            ModelState.AddModelError(string.Empty, $"A user with this email ({Input.Email}) already exists.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "An unexpected error occurred while saving data.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred while creating user.");
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
