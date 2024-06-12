using System.Threading.Tasks;
using WebAPI_Users.Models;

namespace WebAPI_Users.Services
{
    public interface IAuthService
    {
        Task<Persona?> AuthenticateAsync(string correo, string clave);
        Task<bool> CheckEmailExistsAsync(string correo);
        Task<bool> CheckPasswordAsync(string correo, string clave);
        Task<bool> CheckUserStatusAsync(string correo);
    }
}
