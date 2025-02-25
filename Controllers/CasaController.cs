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

        [HttpGet]
        [Description("Obtiene todas las casas con sus residentes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CasaCreateDto>>> GetCasas()
        {
            var casasDto = await _context.Casas
                .Include(c => c.Residentes)
                .Include(c => c.Cuotas)
                .Select(c => new CasaCreateDto
                {
                    IdCasa = c.IdCasa,
                    Direccion = c.Direccion,
                    NumeroCasa = c.NumeroCasa,
                    Residentes = c.Residentes.Select(r => new ResidenteCreateDto
                    {
                        IdResidente = r.IdResidente,
                        Nombre = r.Nombre,
                        Telefono = r.Telefono,
                        Email = r.Email
                    }).ToList(),
                    Cuotas = c.Cuotas.Select(q => new CuotaCreateDto
                    {
                        NombreCuota = q.NombreCuota,
                        Monto = q.Monto,
                        FechaVencimiento = q.FechaVencimiento,
                        Descripcion = q.Descripcion,
                        Estado = q.Estado.ToString(),
                        IdResidente = q.IdResidente
                    }).ToList()
                }).ToListAsync();

            return Ok(casasDto);
        }

        [HttpGet("{id}")]
        [Description("Obtiene una casa específica por su ID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CasaCreateDto>> GetCasa(int id)
        {
            var casa = await _context.Casas
                .Include(c => c.Residentes)
                .Include(c => c.Cuotas)
                .FirstOrDefaultAsync(c => c.IdCasa == id);

            if (casa == null)
                return NotFound(new { mensaje = "Casa no encontrada" });

            var casaDto = new CasaCreateDto
            {
                IdCasa = casa.IdCasa,
                Direccion = casa.Direccion,
                NumeroCasa = casa.NumeroCasa,
                Residentes = casa.Residentes.Select(r => new ResidenteCreateDto
                {
                    IdResidente = r.IdResidente,
                    Nombre = r.Nombre,
                    Telefono = r.Telefono,
                    Email = r.Email
                }).ToList(),
                Cuotas = casa.Cuotas.Select(q => new CuotaCreateDto
                {
                    NombreCuota = q.NombreCuota,
                    Monto = q.Monto,
                    FechaVencimiento = q.FechaVencimiento,
                    Descripcion = q.Descripcion,
                    Estado = q.Estado.ToString(),
                    IdResidente = q.IdResidente
                }).ToList()
            };

            return Ok(casaDto);
        }

        [HttpPost]
        [Description("Crea una nueva casa")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CasaCreateDto>> CreateCasa([FromBody] CasaCreateDto casaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var casa = new Casa
            {
                Direccion = casaDto.Direccion,
                NumeroCasa = casaDto.NumeroCasa,
                Residentes = casaDto.Residentes.Select(r => new Residente
                {
                    Nombre = r.Nombre,
                    Telefono = r.Telefono,
                    Email = r.Email
                }).ToList(),
                Cuotas = casaDto.Cuotas.Select(q =>
                {
                    var estadoDeuda = Enum.TryParse<EstadoDeuda>(q.Estado, out var parsedEstado)
                        ? parsedEstado
                        : EstadoDeuda.Pendiente;
                    return new Cuota
                    {
                        NombreCuota = q.NombreCuota,
                        Monto = q.Monto,
                        FechaVencimiento = q.FechaVencimiento,
                        Descripcion = q.Descripcion,
                        Estado = estadoDeuda,
                        IdResidente = q.IdResidente
                    };
                }).ToList()
            };

            _context.Casas.Add(casa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCasa), new { id = casa.IdCasa }, casaDto);
        }

        [HttpPut("{id}")]
        [Description("Actualiza los datos de una casa existente")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCasa(int id, [FromBody] CasaCreateDto casaDto)
        {
            var casa = await _context.Casas
                .Include(c => c.Residentes)
                .Include(c => c.Cuotas)
                .FirstOrDefaultAsync(c => c.IdCasa == id);

            if (casa == null)
                return NotFound(new { mensaje = "Casa no encontrada" });

            casa.Direccion = casaDto.Direccion ?? casa.Direccion;
            casa.NumeroCasa = casaDto.NumeroCasa ?? casa.NumeroCasa;

            if (casaDto.Residentes != null)
            {
                casa.Residentes.Clear();
                foreach (var residente in casaDto.Residentes)
                {
                    casa.Residentes.Add(new Residente
                    {
                        Nombre = residente.Nombre,
                        Telefono = residente.Telefono,
                        Email = residente.Email
                    });
                }
            }

            if (casaDto.Cuotas != null)
            {
                casa.Cuotas.Clear();
                foreach (var cuota in casaDto.Cuotas)
                {
                    var estadoDeuda = Enum.TryParse<EstadoDeuda>(cuota.Estado, out var parsedEstado)
                        ? parsedEstado
                        : EstadoDeuda.Pendiente;

                    casa.Cuotas.Add(new Cuota
                    {
                        NombreCuota = cuota.NombreCuota,
                        Monto = cuota.Monto,
                        FechaVencimiento = cuota.FechaVencimiento,
                        Descripcion = cuota.Descripcion,
                        Estado = estadoDeuda,
                        IdResidente = cuota.IdResidente
                    });
                }
            }

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

        [HttpGet("debug")]
        public async Task<ActionResult<IEnumerable<Casa>>> GetCasasDebug()
        {
            var casas = await _context.Casas
                .Include(c => c.Residentes)
                .Include(c => c.Cuotas)
                .ToListAsync();
            return Ok(casas);
        }
    }
}
