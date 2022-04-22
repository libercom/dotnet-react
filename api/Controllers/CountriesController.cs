using Microsoft.AspNetCore.Mvc;
using domain.Models;
using core.Repositories.Abstractions;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<ActionResult<IEnumerable<Country>>> GetAllCountries()
        {
            var countries = await _countries.GetAll();

            return countries.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Country>> GetCountry(int id)
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
        }

        [HttpPost]
        public async Task<IActionResult> PostCountry(Country country)
        {
            try
            {
                await _countries.Create(country);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
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

            return NoContent();
        }
    }
}
