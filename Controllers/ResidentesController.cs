using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClusterWeb.Data;
using ClusterWeb.Entities;
using ClusterWeb.DTOs;

namespace ClusterWeb.Controllers
{
    /// <summary>
    /// Controlador para gestionar los residentes
    /// </summary>
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
        /// Obtiene todos los residentes registrados
        /// </summary>
        /// <returns>Lista de residentes</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResidenteCreateDto>>> GetResidentes()
        {
            var residentes = await _context.Residentes
                .Select(r => new ResidenteCreateDto
                {
                    ResidenteId = r.ResidenteId,
                    Nombre = r.Nombre,
                    Telefono = r.Telefono,
                    Email = r.Email,
                    CasaId = r.CasaId,
                    FechaIngreso = r.FechaIngreso,
                    FechaRegistro = r.FechaRegistro
                })
                .ToListAsync();

            return Ok(residentes);
        }

        /// <summary>
        /// Obtiene un residente por su ID
        /// </summary>
        /// <param name="id">ID del residente</param>
        /// <returns>Residente solicitado</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ResidenteCreateDto>> GetResidente(int id)
        {
            var residente = await _context.Residentes.FindAsync(id);

            if (residente == null)
                return NotFound(new { mensaje = "Residente no encontrado" });

            var residenteDto = new ResidenteCreateDto
            {
                ResidenteId = residente.ResidenteId,
                Nombre = residente.Nombre,
                Telefono = residente.Telefono,
                Email = residente.Email,
                CasaId = residente.CasaId,
                FechaIngreso = residente.FechaIngreso,
                FechaRegistro = residente.FechaRegistro
            };

            return Ok(residenteDto);
        }

        /// <summary>
        /// Crea un nuevo residente
        /// </summary>
        /// <param name="ResidenteCreateDto">Datos del residente a crear</param>
        /// <returns>Residente creado</returns>
        [HttpPost]
        public async Task<ActionResult<ResidenteCreateDto>> PostResidente([FromBody] ResidenteCreateDto residenteDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var casaExiste = await _context.Casas.AnyAsync(c => c.CasaId == residenteDto.CasaId);
            if (!casaExiste)
                return NotFound(new { mensaje = "Casa no encontrada" });

            var residente = new Residente
            {
                Nombre = residenteDto.Nombre,
                Telefono = residenteDto.Telefono,
                Email = residenteDto.Email,
                CasaId = residenteDto.CasaId,
                FechaIngreso = DateTime.Now,
                FechaRegistro = DateTime.Now
            };

            _context.Residentes.Add(residente);
            await _context.SaveChangesAsync();

            var nuevoResidenteDto = new ResidenteCreateDto
            {
                ResidenteId = residente.ResidenteId,
                Nombre = residente.Nombre,
                Telefono = residente.Telefono,
                Email = residente.Email,
                CasaId = residente.CasaId,
                FechaIngreso = residente.FechaIngreso,
                FechaRegistro = residente.FechaRegistro
            };

            return CreatedAtAction(nameof(GetResidente), new { id = residente.ResidenteId }, nuevoResidenteDto);
        }

        /// <summary>
        /// Actualiza un residente existente
        /// </summary>
        /// <param name="id">ID del residente a actualizar</param>
        /// <param name="residenteDto">Datos actualizados del residente</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResidente(int id, [FromBody] ResidenteCreateDto residenteDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var residente = await _context.Residentes.FindAsync(id);
            if (residente == null)
                return NotFound(new { mensaje = "Residente no encontrado" });

            var casaExiste = await _context.Casas.AnyAsync(c => c.CasaId == residenteDto.CasaId);
            if (!casaExiste)
                return NotFound(new { mensaje = "Casa no encontrada" });

            residente.Nombre = residenteDto.Nombre;
            residente.Telefono = residenteDto.Telefono;
            residente.Email = residenteDto.Email;
            residente.CasaId = residenteDto.CasaId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResidenteExists(id))
                    return NotFound(new { mensaje = "Residente no encontrado" });
                else throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Elimina un residente
        /// </summary>
        /// <param name="id">ID del residente a eliminar</param>
        /// <returns>No content</returns>
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

        private bool ResidenteExists(int id)
        {
            return _context.Residentes.Any(e => e.ResidenteId == id);
        }
    }
}
