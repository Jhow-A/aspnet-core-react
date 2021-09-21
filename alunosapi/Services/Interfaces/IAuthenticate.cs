using System.Threading.Tasks;

namespace AlunosApi.Services.Interfaces
{
    public interface IAuthenticate
    {
        Task<bool> Authenticate(string email, string password);
        Task Logout();
    }
}
