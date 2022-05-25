using Microsoft.AspNetCore.Mvc;
using core.Repositories.Abstractions;
using Microsoft.AspNetCore.Authorization;
using common.Dtos;

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

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetAllCompanies()
        {
            var companies = await _companies.GetAll();

            return companies.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> GetCompany(int id)
        {
            var company = await _companies.Get(id);

            return company;
        }

        [HttpPost]
        public async Task<IActionResult> PostCompany(CompanyDto company)
        {
            await _companies.Create(company);

            return CreatedAtAction(nameof(GetCompany), new { id = company.CompanyId }, company);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            await _companies.Delete(id);

            return NoContent();
        }
    }
}
