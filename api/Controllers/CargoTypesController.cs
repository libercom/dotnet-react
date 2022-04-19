using core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using core.Context;
using core.Repositories.Abstractions;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CargoTypesController : Controller
    {
        private readonly ICargoTypesRepository _cargoTypes;

        public CargoTypesController(ICargoTypesRepository cargoTypes)
        {
            _cargoTypes = cargoTypes;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CargoType>>> GetAllCargoTypes()
        {
            var cargoTypes = await _cargoTypes.GetAll();

            return cargoTypes.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CargoType>> GetCargoType(int id)
        {
            try
            {
                var cargoType = await _cargoTypes.Get(id);

                return cargoType;
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostCargoType(CargoType cargoType)
        {
            try
            {
                await _cargoTypes.Create(cargoType);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetCargoType), new { id = cargoType.CargoTypeId }, cargoType);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCargoType(int id)
        {
            try
            {
                await _cargoTypes.Delete(id);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
