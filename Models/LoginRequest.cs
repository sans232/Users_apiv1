using System.ComponentModel.DataAnnotations;

namespace WebAPI_Users.Models
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        [Required]
        public string Clave { get; set; }
    }
}
