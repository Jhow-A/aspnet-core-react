using AlunosApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace AlunosApi.Services
{
    public class AuthenticateService : IAuthenticate
    {
        private readonly SignInManager<IdentityRole> _signInManager;
        
        public AuthenticateService(SignInManager<IdentityRole> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<bool> Authenticate(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, lockoutOnFailure: false);
            return result.Succeeded;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
