using Microsoft.AspNetCore.Mvc;
using core.Repositories.Abstractions;
using Microsoft.AspNetCore.Authorization;
using common.Dtos;

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
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CargoTypeDto>>> GetAllCargoTypes()
        {
            var cargoTypes = await _cargoTypes.GetAll();

            return cargoTypes.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CargoTypeDto>> GetCargoType(int id)
        {
            var cargoType = await _cargoTypes.Get(id);

            return cargoType;
        }

        [HttpPost]
        public async Task<IActionResult> PostCargoType(CargoTypeDto cargoType)
        {
            await _cargoTypes.Create(cargoType);

            return CreatedAtAction(nameof(GetCargoType), new { id = cargoType.CargoTypeId }, cargoType);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCargoType(int id)
        {
            await _cargoTypes.Delete(id);

            return NoContent();
        }
    }
}
