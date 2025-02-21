using ClusterWeb.Data;
using ClusterWeb.DTOs;
using ClusterWeb.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClusterWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CasasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CasasController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todas las casas registradas.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CasaCreateDto>>> GetCasas()
        {
            var casas = await _context.Casas
                                    .Include(c => c.Residentes)
                                    .ToListAsync();

            var casasDto = casas.Select(c => new CasaCreateDto
            {
                Direccion = c.Direccion,
                NumeroCasa = c.NumeroCasa,
                Habitaciones = c.Habitaciones,
                Banos = c.Banos,
                FechaRegistro = c.FechaRegistro,
                Residentes = c.Residentes.Select(r => new ResidenteCreateDto 
                {
                    ResidenteId = r.ResidenteId,
                    Nombre = r.Nombre,
                    Telefono = r.Telefono,
                    Email = r.Email,
                    FechaIngreso = r.FechaIngreso,
                    FechaRegistro = r.FechaRegistro
                }).ToList()
            }).ToList();

            return Ok(casasDto);
        }

        /// <summary>
        /// Obtiene una casa por su ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Casa>> GetCasa(int id)
        {
            var casa = await _context.Casas
                                     .Include(c => c.Residentes)
                                     .FirstOrDefaultAsync(c => c.CasaId == id);

            if (casa == null)
                return NotFound(new { mensaje = "Casa no encontrada" });

            return casa;
        }

        /// <summary>
        /// Crea una nueva casa.
        /// </summary>

        [HttpPost]
        public async Task<ActionResult<Casa>> CreateCasa([FromBody] CasaCreateDto casaDto)
        {
            if (!ModelState.IsValid)
              return BadRequest(ModelState);

            var casa = new Casa
            {
                Direccion = casaDto.Direccion,
                NumeroCasa = casaDto.NumeroCasa,
                Habitaciones = casaDto.Habitaciones,
                Banos = casaDto.Banos,
                FechaRegistro = DateTime.Now
            };

            _context.Casas.Add(casa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCasa), new { id = casa.CasaId }, casa);
        }

        /// <summary>
        /// Actualiza los datos de una casa.
        /// </summary>

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCasa(int id, [FromBody] CasaCreateDto casaDto)
        {
            var casa = await _context.Casas.FindAsync(id);
            
            if (casa == null)
                return NotFound(new { mensaje = "Casa no encontrada" });

            // Actualizar solo los campos necesarios
            casa.Direccion = casaDto.Direccion;
            casa.NumeroCasa = casaDto.NumeroCasa;
            casa.Habitaciones = casaDto.Habitaciones;
            casa.Banos = casaDto.Banos;

            _context.Entry(casa).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar la casa" });
            }

            return NoContent(); // Respuesta estándar de actualización exitosa sin contenido
        }


        /// <summary>
        /// Elimina una casa por su ID.
        /// </summary>

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCasa(int id)
        {
            var casa = await _context.Casas.FindAsync(id);
            if (casa == null)
                return NotFound(new { mensaje = "Casa no encontrada" });

            _context.Casas.Remove(casa);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
