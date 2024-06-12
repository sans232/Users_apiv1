using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_Users.Data;
using WebAPI_Users.Models;
namespace WebAPI_Users.Controllers
{
    namespace WebApi.Controllers
    {
        [Route("users/[controller]")]
        [ApiController]
        public class PersonasController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public PersonasController(ApplicationDbContext context)
            {
                _context = context;
            }

            // Método auxiliar para verificar si un registro existe
            private bool PersonaExists(int id)
            {
                return _context.Personas.Any(e => e.IdPersona == id);
            }

            // GET: users/personas
            [HttpGet]
            public async Task<ActionResult<IEnumerable<Persona>>> GetPersonas()
            {
                return await _context.Personas
                .Include(p => p.TipoPersona)
                .Select(p => new Persona
                {
                    IdPersona = p.IdPersona,
                    Nombre = p.Nombre ?? string.Empty,
                    Apellido = p.Apellido ?? string.Empty,
                    Correo = p.Correo ?? string.Empty,
                    Clave = p.Clave ?? string.Empty,
                    Codigo = p.Codigo ?? string.Empty,
                    IdTipoPersona = p.IdTipoPersona,
                    Estado = p.Estado,
                    FechaCreacion = p.FechaCreacion,
                    TipoPersona = p.TipoPersona
                })
                .ToListAsync();
            }

            // GET: users/personas/{id}
            [HttpGet("{id}")]
            public async Task<ActionResult<Persona>> GetPersona(int id)
            {
                var persona = await _context.Personas
                    .Include(p => p.TipoPersona)
                    .Where(p => p.IdPersona == id)
                    .Select(p => new Persona
                    {
                        IdPersona = p.IdPersona,
                        Nombre = p.Nombre ?? string.Empty,
                        Apellido = p.Apellido ?? string.Empty,
                        Correo = p.Correo ?? string.Empty,
                        Clave = p.Clave ?? string.Empty,
                        Codigo = p.Codigo ?? string.Empty,
                        IdTipoPersona = p.IdTipoPersona,
                        Estado = p.Estado,
                        FechaCreacion = p.FechaCreacion,
                        TipoPersona = p.TipoPersona
                    })
                    .FirstOrDefaultAsync();

                if (persona == null)
                {
                    return NotFound(new { message = "El registro con el ID especificado no existe." });
                }

                return persona;
            }

            // PUT: users/personas/{id}
            [HttpPut("{id}")]
            public async Task<IActionResult> PutPersona(int id, Persona persona)
            {
                if (id != persona.IdPersona)
                {
                    return BadRequest(new { message = "El ID de la URL no coincide con el ID de la entidad y no puede ser cambiado." });
                }

                var originalPersona = await _context.Personas.AsNoTracking().FirstOrDefaultAsync(p => p.IdPersona == id);
                if (originalPersona == null)
                {
                    return NotFound(new { message = "La persona con el ID especificado no existe." });
                }

                persona.Codigo = originalPersona.Codigo;
                persona.FechaCreacion = originalPersona.FechaCreacion;

                var updatedFields = new List<string>();

                if (originalPersona.Nombre != persona.Nombre)
                {
                    updatedFields.Add(nameof(persona.Nombre));
                }
                if (originalPersona.Apellido != persona.Apellido)
                {
                    updatedFields.Add(nameof(persona.Apellido));
                }
                if (originalPersona.Correo != persona.Correo)
                {
                    updatedFields.Add(nameof(persona.Correo));
                }
                if (originalPersona.Clave != persona.Clave)
                {
                    updatedFields.Add(nameof(persona.Clave));
                }
                if (originalPersona.IdTipoPersona != persona.IdTipoPersona)
                {
                    updatedFields.Add(nameof(persona.IdTipoPersona));
                }
                if (originalPersona.Estado != persona.Estado)
                {
                    updatedFields.Add(nameof(persona.Estado));
                }

                if (updatedFields.Count == 0)
                {
                    return Ok(new { message = "No se realizaron cambios en la entidad." });
                }

                _context.Entry(persona).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonaExists(id))
                    {
                        return NotFound(new { message = "La persona con el ID especificado no existe." });
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok(new { message = "La persona se actualizó correctamente.", updatedFields });
            }

            // POST: users/personas
            [HttpPost]
            public async Task<ActionResult<Persona>> PostPersona(Persona persona)
            {
                var existingEmail = await _context.Personas.AnyAsync(p => p.Correo == persona.Correo);
                if (existingEmail)
                {
                    return BadRequest(new { message = "El correo ya está registrado." });
                }

                persona.Clave = HashHelper.ComputeSha256Hash(persona.Clave);

                persona.FechaCreacion = DateTime.Now;
                persona.Estado = true;

                _context.Personas.Add(persona);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetPersona", new { id = persona.IdPersona }, persona);
            }

            // DELETE: users/personas/{id}
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeletePersona(int id)
            {
                var persona = await _context.Personas.FindAsync(id);
                if (persona == null)
                {
                    return NotFound(new { message = "El registro con el ID especificado no existe." });
                }

                if (!persona.Estado.GetValueOrDefault())
                {
                    return BadRequest(new { message = "El registro ya ha sido eliminado." });
                }

                persona.Estado = false;
                _context.Entry(persona).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return Ok(new { message = "El registro ha sido eliminado." });
            }
        }
    }
}
