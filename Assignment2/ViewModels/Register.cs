using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Assignment2.ViewModels
{
	public class Register
	{
		[Required]
		[DataType(DataType.Text)]
		public string FirstName { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string LastName { get; set; }

		[Required]
		[DataType(DataType.CreditCard)]
		public string CreditCard { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string MobileNumber { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string BillingAddress { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string ShippingAddress { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password is required.")]
		[DataType(DataType.Password)]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 12)]
		[RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{12,}$", ErrorMessage = "Password must have a combination of lowercase, uppercase, numbers, and special characters.")]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
		public string ConfirmPassword { get; set; }

		[Required]
		[DataType(DataType.Upload)]
		public IFormFile Photo { get; set; }
	}

}
