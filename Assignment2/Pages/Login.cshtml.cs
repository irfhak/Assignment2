using Assignment2.Model;
using Assignment2.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Assignment2.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Login LModel { get; set; }

        private readonly ILogger<LoginModel> _logger;

        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<LoginModel> logger)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(LModel.Email);

                if (user != null)
                {
                    var existingActiveSessions = await userManager.GetLoginsAsync(user);

                    // Log out previous sessions if needed
                    foreach (var existingLogin in existingActiveSessions)
                    {
                        await userManager.RemoveLoginAsync(user, existingLogin.LoginProvider, existingLogin.ProviderKey);
                    }

                    var identityResult = await signInManager.PasswordSignInAsync(user.UserName, LModel.Password, LModel.RememberMe, lockoutOnFailure: true);
                    if (identityResult.Succeeded)
                    {
                        // Store user identifier in the session
                        HttpContext.Session.SetString("UserId", user.Id);

                        _logger.LogInformation($"User {LModel.Email} logged in.");

                        // Optionally set a session timeout (e.g., 20 minutes)
                        HttpContext.Session.SetInt32("SessionTimeout", 20);

                        return RedirectToPage("Index");
                    }
					else if (identityResult.IsLockedOut)
					{
						// Handle account lockout (e.g., show a message to the user)
						ModelState.AddModelError("", "Account locked out due to multiple failed attempts. Try again later.");
						return Page();
					}
					else
                    {
                        _logger.LogWarning($"Failed login attempt for user {LModel.Email}.");
                    }
                }

                ModelState.AddModelError("", "Username or Password incorrect");
            }

            return Page();
        }
    }
}
