using core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using core.Context;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CargoTypesController : Controller
    {
        private readonly CargoDBContext _context;

        public CargoTypesController(CargoDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CargoType>>> GetAllCargoTypes()
        {
            return await _context.CargoTypes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CargoType>> GetCargoType(int id)
        {
            var cargoType = await _context.CargoTypes.FindAsync(id);

            if (cargoType == null)
                return NotFound();

            return cargoType;
        }

        [HttpPost]
        public async Task<IActionResult> PostCargoType(CargoType cargoType)
        {
            if (cargoType == null)
                return BadRequest();

            _context.CargoTypes.Add(cargoType);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCargoType), new { id = cargoType.CargoTypeId }, cargoType);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCargoType(int id)
        {
            var cargoType = await _context.CargoTypes.FindAsync(id);

            if (cargoType == null)
            {
                return NotFound();
            }

            _context.CargoTypes.Remove(cargoType);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
