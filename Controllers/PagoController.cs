using ClusterWeb.Entities;
using ClusterWeb.Data;
using ClusterWeb.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClusterWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PagoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/pago
        [HttpGet]
        public async Task<IActionResult> GetPagos()
        {
            var pagos = await _context.Pagos.ToListAsync();
            var pagoDtos = pagos.Select(p => new PagoDto
            {
                PagoId = p.PagoId,
                DeudaId = p.DeudaId,
                ResidenteId = p.ResidenteId,
                MontoPagado = p.MontoPagado,
                FechaPago = p.FechaPago,
                MetodoPago = p.MetodoPago
            }).ToList();

            return Ok(pagoDtos);
        }

        // GET: api/pago/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPagoById(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null)
            {
                return NotFound();
            }

            var pagoDto = new PagoDto
            {
                PagoId = pago.PagoId,
                DeudaId = pago.DeudaId,
                ResidenteId = pago.ResidenteId,
                MontoPagado = pago.MontoPagado,
                FechaPago = pago.FechaPago,
                MetodoPago = pago.MetodoPago
            };

            return Ok(pagoDto);
        }

        // POST: api/pago
        [HttpPost]
        public async Task<IActionResult> CreatePago([FromBody] PagoCreateDto pagoCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Mapear de DTO a entidad
            var pago = new Pago
            {
                DeudaId = pagoCreateDto.DeudaId,
                ResidenteId = pagoCreateDto.ResidenteId,
                MontoPagado = pagoCreateDto.MontoPagado,
                FechaPago = pagoCreateDto.FechaPago,
                MetodoPago = pagoCreateDto.MetodoPago
            };

            _context.Pagos.Add(pago);
            await _context.SaveChangesAsync();

            // Mapear la entidad creada a DTO para la respuesta
            var pagoDto = new PagoDto
            {
                PagoId = pago.PagoId,
                DeudaId = pago.DeudaId,
                ResidenteId = pago.ResidenteId,
                MontoPagado = pago.MontoPagado,
                FechaPago = pago.FechaPago,
                MetodoPago = pago.MetodoPago
            };

            return CreatedAtAction(nameof(GetPagoById), new { id = pago.PagoId }, pagoDto);
        }

        // PUT: api/pago/{id}
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
                return NotFound();
            }

            // Mapear las propiedades actualizadas del DTO a la entidad
            pago.DeudaId = pagoUpdateDto.DeudaId;
            pago.ResidenteId = pagoUpdateDto.ResidenteId;
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
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/pago/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePago(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null)
            {
                return NotFound();
            }

            _context.Pagos.Remove(pago);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PagoExists(int id)
        {
            return _context.Pagos.Any(e => e.PagoId == id);
        }
    }
}
