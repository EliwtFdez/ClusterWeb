using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClusterWeb.Data;
using ClusterWeb.Entities;
using ClusterWeb.DTOs;

namespace ClusterWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeudaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DeudaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/deuda
        // No funciona
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
                    Estado = d.Estado.ToString(), // Conversión a string
                    Descripcion = d.Descripcion,
                    FechaRegistro = d.FechaRegistro
                })
                .ToListAsync();

            return Ok(deudas);
        }

        // GET: api/deuda/{id}
        // FUNCIONA
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
                Estado = deuda.Estado.ToString(), // Conversión a string
                Descripcion = deuda.Descripcion,
                FechaRegistro = deuda.FechaRegistro
            };

            return Ok(deudaDto);
        }

        // POST: api/deuda
        
        [HttpPost]
        public async Task<ActionResult<DeudaDto>> CreateDeuda([FromBody] DeudaCreateDto deudaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validar que el Residente exista
            var residenteExiste = await _context.Residentes.AnyAsync(r => r.ResidenteId == deudaDto.ResidenteId);
            if (!residenteExiste)
                return NotFound(new { mensaje = "Residente no encontrado" });

            // Validar que la Casa exista
            var casaExiste = await _context.Casas.AnyAsync(c => c.CasaId == deudaDto.CasaId);
            if (!casaExiste)
                return NotFound(new { mensaje = "Casa no encontrada" });

            // Convertir el string del DTO a enum (ignorando mayúsculas/minúsculas)
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
                Estado = deuda.Estado.ToString(), // Conversión a string
                Descripcion = deuda.Descripcion,
                FechaRegistro = deuda.FechaRegistro
            };

            return CreatedAtAction(nameof(GetDeuda), new { id = deuda.DeudaId }, resultDto);
        }

        // PUT: api/deuda/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDeuda(int id, [FromBody] DeudaUpdateDto deudaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var deuda = await _context.Deudas.FindAsync(id);
            if (deuda == null)
                return NotFound(new { mensaje = "Deuda no encontrada" });

            // Convertir el string del DTO a enum (ignorando mayúsculas/minúsculas)
            EstadoDeuda estadoEnum;
            try
            {
                estadoEnum = Enum.Parse<EstadoDeuda>(deudaDto.Estado, ignoreCase: true);
            }
            catch (Exception)
            {
                return BadRequest(new { mensaje = "Estado no válido" });
            }

            // Actualizar los campos permitidos
            deuda.ResidenteId = deudaDto.ResidenteId;
            deuda.CasaId = deudaDto.CasaId;
            deuda.Monto = deudaDto.Monto;
            deuda.SaldoPendiente = deudaDto.SaldoPendiente;
            deuda.FechaVencimiento = deudaDto.FechaVencimiento;
            deuda.Estado = estadoEnum;
            deuda.Descripcion = deudaDto.Descripcion;
            // Normalmente no se actualiza FechaRegistro

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

        // DELETE: api/deuda/{id}
        //FUNCIONA NO TOCAR
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
