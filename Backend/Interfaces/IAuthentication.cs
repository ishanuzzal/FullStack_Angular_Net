using MyProject.Models;

namespace MyProject.Interfaces
{
    public interface IAuthentication
    {
        public string CreateToken(AppUser user, string Email);
        public Task<string> GetUserId(String UserName);

    }
}
