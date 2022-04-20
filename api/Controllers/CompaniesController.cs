using core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using core.Context;
using core.Repositories.Abstractions;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<ActionResult<IEnumerable<Company>>> GetAllCompanies()
        {
            var companies = await _companies.GetAll();

            return companies.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(int id)
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

        }

        [HttpPost]
        public async Task<IActionResult> PostCompany(Company company)
        {
            try
            {
                await _companies.Create(company);
            }
            catch (Exception)
            {
                return BadRequest();
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

            return NoContent();
        }
    }
}
