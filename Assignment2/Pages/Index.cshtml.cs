using Assignment2.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assignment2.Pages
{
	[Authorize]
	public class IndexModel : PageModel
	{

		private readonly UserManager<ApplicationUser> userManager;

		public IndexModel(UserManager<ApplicationUser> userManager)
		{
			this.userManager = userManager;
		}

		public ApplicationUser CurrentUser { get; set; }


        public void OnGet()
		{
            var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
            var protector = dataProtectionProvider.CreateProtector("MySecretKey");

            // Retrieve the currently logged-in user
            var userName = User.Identity.Name;
			CurrentUser = userManager.FindByNameAsync(userName).Result;

            string decryptedCreditCard = protector.Unprotect(CurrentUser.CreditCard);
            ViewData["DecryptedCreditCard"] = decryptedCreditCard;
        }
	}
}