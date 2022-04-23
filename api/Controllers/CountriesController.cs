using Microsoft.AspNetCore.Mvc;
using core.Repositories.Abstractions;
using Microsoft.AspNetCore.Authorization;
using core.Dtos;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CountriesController : Controller
    {
        private readonly ICountriesRepository _countries;

        public CountriesController(ICountriesRepository countries)
        {
            _countries = countries;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetAllCountries()
        {
            try
            {
                var countries = await _countries.GetAll();

                return countries.ToList();
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
            try
            {
                var country = await _countries.Get(id);

                return country;
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
        public async Task<IActionResult> PostCountry(CountryDto country)
        {
            try
            {
                await _countries.Create(country);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return Problem();
            }

            return CreatedAtAction(nameof(GetCountry), new { id = country.CountryId }, country);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            try
            {
                await _countries.Delete(id);
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
