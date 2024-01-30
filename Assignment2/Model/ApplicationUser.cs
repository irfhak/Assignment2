using Microsoft.AspNetCore.Identity;

namespace Assignment2.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CreditCard {  get; set; }
        public string MobileNumber { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public byte[] Photo { get; set; }

        public void SetPhotoFromPath(string imagePath)
        {
            Photo = GetImageBytes(imagePath);
        }

        private byte[] GetImageBytes(string filePath)
        {
            byte[] imageBytes;

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                {
                    imageBytes = binaryReader.ReadBytes((int)fileStream.Length);
                }
            }

            return imageBytes;
        }

    }
}
