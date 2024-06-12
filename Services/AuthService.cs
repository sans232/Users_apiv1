using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebAPI_Users.Data;
using WebAPI_Users.Models;

namespace WebAPI_Users.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Persona?> AuthenticateAsync(string correo, string clave)
        {
            var user = await _context.Personas
                .FirstOrDefaultAsync(p => p.Correo == correo);

            if (user == null || user.Clave != HashHelper.ComputeSha256Hash(clave) || user.Estado != true)
            {
                return null;
            }

            return user;
        }

        public async Task<bool> CheckEmailExistsAsync(string correo)
        {
            return await _context.Personas.AnyAsync(p => p.Correo == correo);
        }

        public async Task<bool> CheckPasswordAsync(string correo, string clave)
        {
            var user = await _context.Personas.FirstOrDefaultAsync(p => p.Correo == correo);
            if (user == null)
            {
                return false;
            }

            var hashedPassword = HashHelper.ComputeSha256Hash(clave);
            return user.Clave == hashedPassword;
        }

        public async Task<bool> CheckUserStatusAsync(string correo)
        {
            var user = await _context.Personas.FirstOrDefaultAsync(p => p.Correo == correo);
            if (user == null)
            {
                return false;
            }

            return user.Estado == true;
        }
    }
}
