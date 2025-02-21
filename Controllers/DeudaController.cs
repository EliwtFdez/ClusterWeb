using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClusterWeb.Data;
using ClusterWeb.Entities;
using ClusterWeb.DTOs;

namespace ClusterWeb.Controllers
{
    /// <summary>
    /// Controlador para gestionar las deudas
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DeudaController : ControllerBase
    {
        private readonly AppDbContext _context;


        public DeudaController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todas las deudas
        /// </summary>
        /// <returns>Lista de deudas</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeudaDto>>> GetDeudas()
        {
            var deudas = await _context.Deudas
                .Select(d => new DeudaDto
                {
                    DeudaId = d.DeudaId,
                    ResidenteId = d.ResidenteId,
                    CasaId = d.CasaId,
                    Monto = d.Monto,
                    SaldoPendiente = d.SaldoPendiente,
                    FechaVencimiento = d.FechaVencimiento,
                    Estado = d.Estado.ToString(),
                    Descripcion = d.Descripcion,
                    FechaRegistro = d.FechaRegistro
                })
                .ToListAsync();

            return Ok(deudas);
        }

        /// <summary>
        /// Obtiene una deuda específica por su ID
        /// </summary>
        /// <param name="id">ID de la deuda</param>
        /// <returns>Deuda solicitada</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<DeudaDto>> GetDeuda(int id)
        {
            var deuda = await _context.Deudas.FindAsync(id);

            if (deuda == null)
                return NotFound(new { mensaje = "Deuda no encontrada" });

            var deudaDto = new DeudaDto
            {
                DeudaId = deuda.DeudaId,
                ResidenteId = deuda.ResidenteId,
                CasaId = deuda.CasaId,
                Monto = deuda.Monto,
                SaldoPendiente = deuda.SaldoPendiente,
                FechaVencimiento = deuda.FechaVencimiento,
                Estado = deuda.Estado.ToString(),
                Descripcion = deuda.Descripcion,
                FechaRegistro = deuda.FechaRegistro
            };

            return Ok(deudaDto);
        }

        /// <summary>
        /// Crea una nueva deuda
        /// </summary>
        /// <param name="deudaDto">Datos de la deuda a crear</param>
        /// <returns>Deuda creada</returns>
        [HttpPost]
        public async Task<ActionResult<DeudaDto>> CreateDeuda([FromBody] DeudaCreateDto deudaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var residenteExiste = await _context.Residentes.AnyAsync(r => r.ResidenteId == deudaDto.ResidenteId);
            if (!residenteExiste)
                return NotFound(new { mensaje = "Residente no encontrado" });

            var casaExiste = await _context.Casas.AnyAsync(c => c.CasaId == deudaDto.CasaId);
            if (!casaExiste)
                return NotFound(new { mensaje = "Casa no encontrada" });

            EstadoDeuda estadoEnum;
            try
            {
                estadoEnum = Enum.Parse<EstadoDeuda>(deudaDto.Estado, ignoreCase: true);
            }
            catch (Exception)
            {
                return BadRequest(new { mensaje = "Estado no válido" });
            }

            var deuda = new Deuda
            {
                ResidenteId = deudaDto.ResidenteId,
                CasaId = deudaDto.CasaId,
                Monto = deudaDto.Monto,
                SaldoPendiente = deudaDto.SaldoPendiente,
                FechaVencimiento = deudaDto.FechaVencimiento,
                Estado = estadoEnum,
                Descripcion = deudaDto.Descripcion,
                FechaRegistro = DateTime.Now
            };

            _context.Deudas.Add(deuda);
            await _context.SaveChangesAsync();

            var resultDto = new DeudaDto
            {
                DeudaId = deuda.DeudaId,
                ResidenteId = deuda.ResidenteId,
                CasaId = deuda.CasaId,
                Monto = deuda.Monto,
                SaldoPendiente = deuda.SaldoPendiente,
                FechaVencimiento = deuda.FechaVencimiento,
                Estado = deuda.Estado.ToString(),
                Descripcion = deuda.Descripcion,
                FechaRegistro = deuda.FechaRegistro
            };

            return CreatedAtAction(nameof(GetDeuda), new { id = deuda.DeudaId }, resultDto);
        }

        /// <summary>
        /// Actualiza una deuda existente
        /// </summary>
        /// <param name="id">ID de la deuda a actualizar</param>
        /// <param name="deudaDto">Nuevos datos de la deuda</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDeuda(int id, [FromBody] DeudaUpdateDto deudaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var deuda = await _context.Deudas.FindAsync(id);
            if (deuda == null)
                return NotFound(new { mensaje = "Deuda no encontrada" });

            EstadoDeuda estadoEnum;
            try
            {
                estadoEnum = Enum.Parse<EstadoDeuda>(deudaDto.Estado, ignoreCase: true);
            }
            catch (Exception)
            {
                return BadRequest(new { mensaje = "Estado no válido" });
            }

            deuda.ResidenteId = deudaDto.ResidenteId;
            deuda.CasaId = deudaDto.CasaId;
            deuda.Monto = deudaDto.Monto;
            deuda.SaldoPendiente = deudaDto.SaldoPendiente;
            deuda.FechaVencimiento = deudaDto.FechaVencimiento;
            deuda.Estado = estadoEnum;
            deuda.Descripcion = deudaDto.Descripcion;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Deudas.Any(d => d.DeudaId == id))
                    return NotFound(new { mensaje = "Deuda no encontrada" });
                else
                    throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Elimina una deuda
        /// </summary>
        /// <param name="id">ID de la deuda a eliminar</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeuda(int id)
        {
            var deuda = await _context.Deudas.FindAsync(id);
            if (deuda == null)
                return NotFound(new { mensaje = "Deuda no encontrada" });

            _context.Deudas.Remove(deuda);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
