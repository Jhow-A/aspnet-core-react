using System.Threading.Tasks;

namespace AlunosApi.Services.Interfaces
{
    public interface IAuthenticate
    {
        Task<bool> AuthenticateUser(string email, string password);
        Task<bool> RegisterUser(string email, string password);
        Task Logout();
    }
}
