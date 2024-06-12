namespace WebAPI_Users.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using WebAPI_Users.Data;
    using WebAPI_Users.Models;

    [Route("users/[controller]")]
    [ApiController]
    public class TipoPersonasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TipoPersonasController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool TipoPersonaExists(int id)
        {
            return _context.TipoPersonas.Any(e => e.IdTipoPersona == id);
        }

        // GET: users/tipopersonas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoPersona>>> GetTipoPersonas()
        {
            return await _context.TipoPersonas.ToListAsync();
        }

        // GET: users/tipopersonas/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoPersona>> GetTipoPersona(int id)
        {
            var tipoPersona = await _context.TipoPersonas.FindAsync(id);

            if (tipoPersona == null)
            {
                return NotFound(new { message = "El tipo de persona con el ID especificado no existe." });
            }

            return tipoPersona;
        }

        // PUT: users/tipopersonas/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoPersona(int id, TipoPersona tipoPersona)
        {
            if (id != tipoPersona.IdTipoPersona)
            {
                return BadRequest(new { message = "El ID de la URL no coincide con el ID de la entidad y no puede ser cambiado." });
            }

            var existingTipoPersona = await _context.TipoPersonas.AsNoTracking().FirstOrDefaultAsync(tp => tp.IdTipoPersona == id);
            if (existingTipoPersona == null)
            {
                return NotFound(new { message = "El tipo de persona con el ID especificado no existe." });
            }

            // Mantener el valor de FechaCreacion del registro existente
            tipoPersona.FechaCreacion = existingTipoPersona.FechaCreacion;

            var updatedFields = new List<string>();

            if (existingTipoPersona.Descripcion != tipoPersona.Descripcion)
            {
                updatedFields.Add(nameof(tipoPersona.Descripcion));
            }
            if (existingTipoPersona.Estado != tipoPersona.Estado)
            {
                updatedFields.Add(nameof(tipoPersona.Estado));
            }

            if (updatedFields.Count == 0)
            {
                return Ok(new { message = "No se realizaron cambios en la entidad." });
            }

            _context.Entry(existingTipoPersona).CurrentValues.SetValues(tipoPersona);
            _context.Entry(existingTipoPersona).Property(e => e.FechaCreacion).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoPersonaExists(id))
                {
                    return NotFound(new { message = "El tipo de persona con el ID especificado no existe." });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "El tipo de persona se actualizó correctamente.", updatedFields });
        }

        // POST: users/tipopersonas
        [HttpPost]
        public async Task<ActionResult<TipoPersona>> PostTipoPersona(TipoPersona tipoPersona)
        {
            var existingDescription = await _context.TipoPersonas.AnyAsync(tp => tp.Descripcion == tipoPersona.Descripcion);
            if (existingDescription)
            {
                return BadRequest(new { message = "La descripción ya está registrada." });
            }

            tipoPersona.FechaCreacion = DateTime.Now;

            _context.TipoPersonas.Add(tipoPersona);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoPersona", new { id = tipoPersona.IdTipoPersona }, tipoPersona);
        }


        // DELETE: users/tipopersonas/{id}]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoPersona(int id)
        {
            var tipoPersona = await _context.TipoPersonas.FindAsync(id);
            if (tipoPersona == null)
            {
                return NotFound(new { message = "El tipo de persona con el ID especificado no existe." });
            }

            if (!tipoPersona.Estado.GetValueOrDefault())
            {
                return BadRequest(new { message = "El tipo de persona ya ha sido eliminado." });
            }

            _context.TipoPersonas.Remove(tipoPersona);
            await _context.SaveChangesAsync();

            return Ok(new { message = "El tipo de persona ha sido eliminado." });
        }
    }
}
