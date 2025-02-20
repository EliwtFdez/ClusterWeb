using ClusterWeb.Entities;
using ClusterWeb.Data;
using Microsoft.AspNetCore.Mvc;

namespace ClusterWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeudaController : ControllerBase
    {
        // GET: api/deuda
        [HttpGet]
        public IActionResult GetAllDeudas()
        {
            // Logic to get all deudas
            return Ok(new { message = "Get all deudas" });
        }

        // GET: api/deuda/{id}
        [HttpGet("{id}")]
        public IActionResult GetDeudaById(int id)
        {
            // Logic to get a deuda by id
            return Ok(new { message = $"Get deuda with id {id}" });
        }

        // POST: api/deuda
        [HttpPost]
        public IActionResult CreateDeuda([FromBody] Deuda deuda)
        {
            // Logic to create a new deuda
            return Ok(new { message = "Deuda created" });
        }

        // PUT: api/deuda/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateDeuda(int id, [FromBody] Deuda deuda)
        {
            // Logic to update an existing deuda
            return Ok(new { message = $"Deuda with id {id} updated" });
        }

        // DELETE: api/deuda/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteDeuda(int id)
        {
            // Logic to delete a deuda
            return Ok(new { message = $"Deuda with id {id} deleted" });
        }
    }

}