using Microsoft.AspNetCore.Mvc;
using core.Repositories.Abstractions;
using Microsoft.AspNetCore.Authorization;
using core.Dtos;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CargoTypesController : Controller
    {
        private readonly ICargoTypesRepository _cargoTypes;

        public CargoTypesController(ICargoTypesRepository cargoTypes)
        {
            _cargoTypes = cargoTypes;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CargoTypeDto>>> GetAllCargoTypes()
        {
            try
            {
                var cargoTypes = await _cargoTypes.GetAll();

                return cargoTypes.ToList();
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CargoTypeDto>> GetCargoType(int id)
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
            catch (Exception)
            {
                return Problem();
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostCargoType(CargoTypeDto cargoType)
        {
            try
            {
                await _cargoTypes.Create(cargoType);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return Problem();
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
            catch (Exception)
            {
                return Problem();
            }

            return NoContent();
        }
    }
}
