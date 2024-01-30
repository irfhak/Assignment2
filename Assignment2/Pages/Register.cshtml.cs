using Assignment2.Model;
using Assignment2.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assignment2.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<RegisterModel> _logger; // Corrected type

        [BindProperty]
        public Register RModel { get; set; }

        public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<RegisterModel> logger) // Corrected type
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public void OnGet()
        {
        }

        //Save data into the database
        public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.IsValid)
			{
				var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
				var protector = dataProtectionProvider.CreateProtector("MySecretKey");

				var user = new ApplicationUser()
				{
					UserName = RModel.Email,
					Email = RModel.Email,
					FirstName = RModel.FirstName,
					LastName = RModel.LastName,
					CreditCard = protector.Protect(RModel.CreditCard),
					MobileNumber = RModel.MobileNumber,
					BillingAddress = RModel.BillingAddress,
					ShippingAddress = RModel.ShippingAddress,
				};

                if (RModel.Photo != null && RModel.Photo.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await RModel.Photo.CopyToAsync(memoryStream);
                        user.Photo = memoryStream.ToArray();
                    }
                }

                var result = await _userManager.CreateAsync(user, RModel.Password);
				if (result.Succeeded)
				{
					await _signInManager.SignInAsync(user, false);
					return RedirectToPage("Index");
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}

			}
			return Page();
		}
	}
}
