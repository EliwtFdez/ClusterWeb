using Microsoft.AspNetCore.Mvc;
using ClusterWeb.Data;
using ClusterWeb.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        // Obtener todos los residentes
        [HttpGet]
        public ActionResult<IEnumerable<Residente>> GetResidentes()
        {
            return Ok(_context.Residentes.ToList());
        }

        // Insertar un nuevo residente
        [HttpPost]
        public async Task<ActionResult<Residente>> PostResidente(Residente residente)
        {
            _context.Residentes.Add(residente);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetResidentes), new { id = residente.ResidenteId }, residente);
        }
    }
}
