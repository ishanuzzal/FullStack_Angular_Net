using Microsoft.AspNetCore.Identity;

namespace MyProject.Models
{
    public class AppUser:IdentityUser
    {
        public List<Product> products { get; set; }

    }
}
