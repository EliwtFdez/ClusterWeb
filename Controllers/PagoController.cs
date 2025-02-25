using ClusterWeb.Entities;
using ClusterWeb.Data;
using ClusterWeb.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClusterWeb.Controllers
{
    /// <summary>
    /// Controlador para gestionar los pagos
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PagoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PagoController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los pagos
        /// </summary>
        /// <returns>Lista de pagos</returns>
        [HttpGet]
        public async Task<IActionResult> GetPagos()
        {
            var pagos = await _context.Pagos
                .Include(p => p.Cuota)
                .ToListAsync();

            var pagoDtos = pagos.Select(p => new PagoDto
            {
                IdPago = p.IdPago,
                IdCuota = p.IdCuota,
                MontoPagado = p.MontoPagado,
                FechaPago = p.FechaPago,
                MetodoPago = p.MetodoPago,
            }).ToList();

            return Ok(pagoDtos);
        }

        /// <summary>
        /// Obtiene un pago específico por su ID
        /// </summary>
        /// <param name="id">ID del pago</param>
        /// <returns>Pago solicitado</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPagoById(int id)
        {
            var pago = await _context.Pagos
                .Include(p => p.Cuota)
                .FirstOrDefaultAsync(p => p.IdPago == id);
            if (pago == null)
            {
                return NotFound(new { mensaje = "Pago no encontrado" });
            }

            var pagoDto = new PagoDto
            {
                IdPago = pago.IdPago,
                IdCuota = pago.IdCuota,
                MontoPagado = pago.MontoPagado,
                FechaPago = pago.FechaPago,
                MetodoPago = pago.MetodoPago,
            };

            return Ok(pagoDto);
        }

        /// <summary>
        /// Crea un nuevo pago
        /// </summary>
        /// <param name="pagoCreateDto">Datos del pago a crear</param>
        /// <returns>Pago creado</returns>
        [HttpPost]
        public async Task<IActionResult> CreatePago([FromBody] PagoCreateDto pagoCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var deudaExiste = await _context.Cuotas.AnyAsync(d => d.IdCuota == pagoCreateDto.IdCuota);
            if (!deudaExiste)
                return NotFound(new { mensaje = "Deuda no encontrada" });

            var pago = new Pago
            {
                IdCuota = pagoCreateDto.IdCuota,
                MontoPagado = pagoCreateDto.MontoPagado,
                FechaPago = pagoCreateDto.FechaPago,
                MetodoPago = pagoCreateDto.MetodoPago,
            };

            _context.Pagos.Add(pago);
            await _context.SaveChangesAsync();

            var pagoDto = new PagoDto
            {
                IdPago = pago.IdPago,
                IdCuota = pago.IdCuota,
                MontoPagado = pago.MontoPagado,
                FechaPago = pago.FechaPago,
                MetodoPago = pago.MetodoPago,
            };

            return CreatedAtAction(nameof(GetPagoById), new { id = pago.IdPago }, pagoDto);
        }

        /// <summary>
        /// Actualiza un pago existente
        /// </summary>
        /// <param name="id">ID del pago a actualizar</param>
        /// <param name="pagoUpdateDto">Datos actualizados del pago</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePago(int id, [FromBody] PagoUpdateDto pagoUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null)
            {
                return NotFound(new { mensaje = "Pago no encontrado" });
            }

            var deudaExiste = await _context.Cuotas.AnyAsync(d => d.IdCuota == pagoUpdateDto.IdCuota);
            if (!deudaExiste)
                return NotFound(new { mensaje = "Deuda no encontrada" });

            pago.IdCuota = pagoUpdateDto.IdCuota;
            pago.MontoPagado = pagoUpdateDto.MontoPagado;
            pago.FechaPago = pagoUpdateDto.FechaPago;
            pago.MetodoPago = pagoUpdateDto.MetodoPago;

            _context.Entry(pago).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PagoExists(id))
                    return NotFound(new { mensaje = "Pago no encontrado" });
                else throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Elimina un pago
        /// </summary>
        /// <param name="id">ID del pago a eliminar</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePago(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null)
            {
                return NotFound(new { mensaje = "Pago no encontrado" });
            }

            _context.Pagos.Remove(pago);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PagoExists(int id)
        {
            return _context.Pagos.Any(e => e.IdPago == id);
        }
    }
}