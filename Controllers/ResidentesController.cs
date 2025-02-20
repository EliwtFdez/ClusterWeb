using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClusterWeb.Data;
using ClusterWeb.Entities;
using ClusterWeb.DTOs;

namespace ClusterWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResidentesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ResidentesController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los residentes registrados.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Residente>>> GetResidentes()
        {
            return await _context.Residentes.ToListAsync();
        }

        /// <summary>
        /// Obtiene un residente por su ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Residente>> GetResidente(int id)
        {
            var residente = await _context.Residentes.FindAsync(id);

            if (residente == null)
                return NotFound(new { mensaje = "Residente no encontrado" });

            return residente;
        }

        /// <summary>
        /// Crea un nuevo residente.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Residente>> PostResidente([FromBody] ResidenteCreateDto residenteDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Opcional: validar que la Casa existe antes de asignarla
            var casaExiste = await _context.Casas.AnyAsync(c => c.CasaId == residenteDto.CasaId);
            if (!casaExiste)
                return NotFound(new { mensaje = "Casa no encontrada" });

            var residente = new Residente
            {
                Nombre = residenteDto.Nombre,
                Telefono = residenteDto.Telefono,
                Email = residenteDto.Email,
                CasaId = residenteDto.CasaId, // Asignación de la clave foránea
                FechaIngreso = DateTime.Now,
                FechaRegistro = DateTime.Now
            };

            _context.Residentes.Add(residente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetResidente), new { id = residente.ResidenteId }, residente);
        }


        /// <summary>
        /// Actualiza un residente existente.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResidente(int id, [FromBody] ResidenteCreateDto residenteDto)
        {
            var residente = await _context.Residentes.FindAsync(id);
            if (residente == null)
                return NotFound(new { mensaje = "Residente no encontrado" });

            // Actualizar los valores permitidos desde el DTO
            residente.Nombre = residenteDto.Nombre;
            residente.Telefono = residenteDto.Telefono;
            residente.Email = residenteDto.Email;
            residente.CasaId = residenteDto.CasaId; // Actualizamos la relación

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }


        /// <summary>
        /// Elimina un residente por su ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResidente(int id)
        {
            var residente = await _context.Residentes.FindAsync(id);
            if (residente == null)
                return NotFound(new { mensaje = "Residente no encontrado" });

            _context.Residentes.Remove(residente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
