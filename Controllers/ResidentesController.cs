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
            .Include(r => r.Casa) // Asegura que se cargue la relación con Casa
            .Select(r => new ResidenteCreateDto
            {
                IdResidente = r.IdResidente,
                Nombre = r.Nombre,
                Telefono = r.Telefono,
                Email = r.Email,
                IdCasa = r.IdCasa // Esto ya debería reflejar correctamente el valor
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
            var residente = await _context.Residentes
                .Include(r => r.Casa) // Asegura que se cargue la relación con Casa
                .FirstOrDefaultAsync(r => r.IdResidente == id);

            if (residente == null)
                return NotFound(new { mensaje = "Residente no encontrado" });

            var residenteDto = new ResidenteCreateDto
            {
                IdResidente = residente.IdResidente,
                Nombre = residente.Nombre,
                Telefono = residente.Telefono,
                Email = residente.Email,
                IdCasa = residente.IdCasa // Esto ya debería reflejar correctamente el valor
            };

            return Ok(residenteDto);
        }


        /// <summary>
        /// Crea un nuevo residente
        /// </summary>
        /// <param name="residenteDto">Datos del residente a crear</param>
        /// <returns>Residente creado</returns>
       [HttpPost]
        public async Task<ActionResult<ResidenteCreateDto>> PostResidente([FromBody] ResidenteCreateDto residenteDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // ⚠️ Verificar si la casa existe en la base de datos antes de crear el residente
            var casaExiste = await _context.Casas.AnyAsync(c => c.IdCasa == residenteDto.IdCasa);
            if (!casaExiste)
                return NotFound(new { mensaje = $"No existe una casa con IdCasa = {residenteDto.IdCasa}" });

            var residente = new Residente
            {
                Nombre = residenteDto.Nombre,
                Telefono = residenteDto.Telefono,
                Email = residenteDto.Email,
                IdCasa = residenteDto.IdCasa // Asegurar que se asigne correctamente
            };

            _context.Residentes.Add(residente);
            await _context.SaveChangesAsync();

            var nuevoResidenteDto = new ResidenteCreateDto
            {
                IdResidente = residente.IdResidente,
                Nombre = residente.Nombre,
                Telefono = residente.Telefono,
                Email = residente.Email,
                IdCasa = residente.IdCasa
            };

            return CreatedAtAction(nameof(GetResidente), new { id = residente.IdResidente }, nuevoResidenteDto);
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

            // ⚠️ Verificar si la casa existe en la base de datos antes de actualizar
            if (residenteDto.IdCasa != null)
            {
                var casaExiste = await _context.Casas.AnyAsync(c => c.IdCasa == residenteDto.IdCasa);
                if (!casaExiste)
                    return NotFound(new { mensaje = $"No existe una casa con IdCasa = {residenteDto.IdCasa}" });
                
                // Asignar el nuevo IdCasa solo si existe
                residente.IdCasa = residenteDto.IdCasa;
            }

            residente.Nombre = residenteDto.Nombre;
            residente.Telefono = residenteDto.Telefono;
            residente.Email = residenteDto.Email;

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
            return _context.Residentes.Any(e => e.IdResidente == id);
        }
    }
}
