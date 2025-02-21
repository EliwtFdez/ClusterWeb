using ClusterWeb.Data;
using ClusterWeb.DTOs;
using ClusterWeb.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

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
        /// Obtiene todas las casas con sus residentes
        /// </summary>
        /// <returns>Lista de casas con información de residentes</returns>
        [HttpGet]
        [Description("Obtiene todas las casas con sus residentes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
        /// Obtiene una casa específica por su ID
        /// </summary>
        /// <param name="id">ID de la casa</param>
        /// <returns>Información detallada de la casa</returns>
        [HttpGet("{id}")]
        [Description("Obtiene una casa específica por su ID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        /// Crea una nueva casa
        /// </summary>
        /// <param name="casaDto">Datos de la casa a crear</param>
        /// <returns>Casa creada</returns>
        [HttpPost]
        [Description("Crea una nueva casa")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        /// Actualiza los datos de una casa existente
        /// </summary>
        /// <param name="id">ID de la casa a actualizar</param>
        /// <param name="casaDto">Nuevos datos de la casa</param>
        /// <returns>No content si la actualización es exitosa</returns>
        [HttpPut("{id}")]
        [Description("Actualiza los datos de una casa existente")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCasa(int id, [FromBody] CasaCreateDto casaDto)
        {
            var casa = await _context.Casas.FindAsync(id);
            
            if (casa == null)
                return NotFound(new { mensaje = "Casa no encontrada" });

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

            return NoContent();
        }

        /// <summary>
        /// Elimina una casa por su ID
        /// </summary>
        /// <param name="id">ID de la casa a eliminar</param>
        /// <returns>No content si la eliminación es exitosa</returns>
        [HttpDelete("{id}")]
        [Description("Elimina una casa por su ID")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
