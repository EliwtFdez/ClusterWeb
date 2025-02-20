using Microsoft.AspNetCore.Mvc;
using ClusterWeb.Data;
using ClusterWeb.Entities;

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

        // 🔹 Obtener todas las casas
        [HttpGet]
        public ActionResult<IEnumerable<Casa>> GetCasas()
        {
            return Ok(_context.Casas.ToList());
        }

        // 🔹 Obtener una casa por ID
        [HttpGet("{id}")]
        public ActionResult<Casa> GetCasa(int id)
        {
            var casa = _context.Casas.Find(id);
            if (casa == null) return NotFound();
            return Ok(casa);
        }

        // 🔹 Insertar una nueva casa
        [HttpPost]
        public async Task<ActionResult<Casa>> PostCasa(Casa casa)
        {
            _context.Casas.Add(casa);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCasa), new { id = casa.CasaId }, casa);
        }
    }
}
