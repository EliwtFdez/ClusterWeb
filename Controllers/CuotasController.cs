using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClusterWeb.Data;
using ClusterWeb.Entities;
using ClusterWeb.DTOs;

namespace ClusterWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuotasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CuotasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CuotaDto>>> GetCuotas()
        {
            var cuotas = await _context.Cuotas
                .Select(c => new CuotaDto
                {
                    IdCuota = c.IdCuota,
                    IdResidente = c.IdResidente,
                    IdCasa = c.IdCasa,
                    Monto = c.Monto,
                    FechaVencimiento = c.FechaVencimiento,
                    Estado = c.Estado.ToString(),
                    Descripcion = c.Descripcion,
                    FechaRegistro = c.FechaRegistro
                })
                .ToListAsync();

            return Ok(cuotas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CuotaDto>> GetCuota(int id)
        {
            var cuota = await _context.Cuotas.FindAsync(id);

            if (cuota == null)
                return NotFound(new { mensaje = "Cuota no encontrada" });

            var cuotaDto = new CuotaDto
            {
                IdCuota = cuota.IdCuota,
                IdResidente = cuota.IdResidente,
                IdCasa = cuota.IdCasa,
                Monto = cuota.Monto,
                FechaVencimiento = cuota.FechaVencimiento,
                Estado = cuota.Estado.ToString(),
                Descripcion = cuota.Descripcion,
                FechaRegistro = cuota.FechaRegistro
            };

            return Ok(cuotaDto);
        }

        [HttpPost]
        public async Task<ActionResult<CuotaDto>> CreateCuota([FromBody] CuotaCreateDto cuotaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var residenteExiste = await _context.Residentes.AnyAsync(r => r.IdResidente == cuotaDto.IdResidente);
            if (!residenteExiste)
                return NotFound(new { mensaje = "Residente no encontrado" });

            var casaExiste = await _context.Casas.AnyAsync(c => c.IdCasa == cuotaDto.IdCasa);
            if (!casaExiste)
                return NotFound(new { mensaje = "Casa no encontrada" });

            EstadoDeuda estadoEnum;
            try
            {
                estadoEnum = Enum.Parse<EstadoDeuda>(cuotaDto.Estado, ignoreCase: true);
            }
            catch (Exception)
            {
                return BadRequest(new { mensaje = "Estado no válido" });
            }

            var cuota = new Cuota
            {
                IdResidente = cuotaDto.IdResidente,
                IdCasa = cuotaDto.IdCasa,
                Monto = cuotaDto.Monto,
                FechaVencimiento = cuotaDto.FechaVencimiento,
                Estado = estadoEnum,
                Descripcion = cuotaDto.Descripcion,
                FechaRegistro = DateTime.Now
            };

            _context.Cuotas.Add(cuota);
            await _context.SaveChangesAsync();

            var resultDto = new CuotaDto
            {
                IdCuota = cuota.IdCuota,
                IdResidente = cuota.IdResidente,
                IdCasa = cuota.IdCasa,
                Monto = cuota.Monto,
                FechaVencimiento = cuota.FechaVencimiento,
                Estado = cuota.Estado.ToString(),
                Descripcion = cuota.Descripcion,
                FechaRegistro = cuota.FechaRegistro
            };

            return CreatedAtAction(nameof(GetCuota), new { id = cuota.IdCuota }, resultDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCuota(int id, [FromBody] CuotaUpdateDto cuotaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cuota = await _context.Cuotas.FindAsync(id);
            if (cuota == null)
                return NotFound(new { mensaje = "Cuota no encontrada" });

            EstadoDeuda estadoEnum;
            try
            {
                estadoEnum = Enum.Parse<EstadoDeuda>(cuotaDto.Estado, ignoreCase: true);
            }
            catch (Exception)
            {
                return BadRequest(new { mensaje = "Estado no válido" });
            }

            cuota.IdResidente = cuotaDto.IdResidente;
            cuota.IdCasa = cuotaDto.IdCasa;
            cuota.Monto = cuotaDto.Monto;
            cuota.FechaVencimiento = cuotaDto.FechaVencimiento;
            cuota.Estado = estadoEnum;
            cuota.Descripcion = cuotaDto.Descripcion;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Cuotas.Any(c => c.IdCuota == id))
                    return NotFound(new { mensaje = "Cuota no encontrada" });
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCuota(int id)
        {
            var cuota = await _context.Cuotas.FindAsync(id);
            if (cuota == null)
                return NotFound(new { mensaje = "Cuota no encontrada" });

            _context.Cuotas.Remove(cuota);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
