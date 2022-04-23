using Microsoft.AspNetCore.Mvc;
using core.Repositories.Abstractions;
using Microsoft.AspNetCore.Authorization;
using core.Dtos;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CompaniesController : Controller
    {
        private readonly ICompaniesRepository _companies;

        public CompaniesController(ICompaniesRepository companies)
        {
            _companies = companies;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetAllCompanies()
        {
            try
            {
                var companies = await _companies.GetAll();

                return companies.ToList();
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> GetCompany(int id)
        {
            try
            {
                var company = await _companies.Get(id);

                return company;
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
        public async Task<IActionResult> PostCompany(CompanyDto company)
        {
            try
            {
                await _companies.Create(company);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return Problem();
            }

            return CreatedAtAction(nameof(GetCompany), new { id = company.CompanyId }, company);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            try
            {
                await _companies.Delete(id);
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
