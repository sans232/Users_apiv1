using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebAPI_Users.Models;
namespace WebAPI_Users.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Persona> Personas { get; set; }
        public DbSet<TipoPersona> TipoPersonas { get; set; }
    }
}
