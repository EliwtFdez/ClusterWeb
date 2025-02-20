using ClusterWeb.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ClusterWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagoController : ControllerBase
    {
        // GET: api/pago
        [HttpGet]
        public IActionResult GetPagos()
        {
            // Logic to get all pagos
            return Ok(new { message = "Get all pagos" });
        }

        // GET: api/pago/{id}
        [HttpGet("{id}")]
        public IActionResult GetPagoById(int id)
        {
            // Logic to get a pago by id
            return Ok(new { message = $"Get pago with id {id}" });
        }

        // POST: api/pago
        [HttpPost]
        public IActionResult CreatePago([FromBody] Pago pago)
        {
            // Logic to create a new pago
            return Ok(new { message = "Pago created" });
        }

        // PUT: api/pago/{id}
        [HttpPut("{id}")]
        public IActionResult UpdatePago(int id, [FromBody] Pago pago)
        {
            // Logic to update an existing pago
            return Ok(new { message = $"Pago with id {id} updated" });
        }

        // DELETE: api/pago/{id}
        [HttpDelete("{id}")]
        public IActionResult DeletePago(int id)
        {
            // Logic to delete a pago
            return Ok(new { message = $"Pago with id {id} deleted" });
        }
    }
}