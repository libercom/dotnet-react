using domain.Context;
using domain.Models;
using core.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace core.Repositories
{
    public class CompaniesRepository : ICompaniesRepository
    {
        private readonly CargoDBContext _context;

        public CompaniesRepository(CargoDBContext context)
        {
            _context = context;
        }

        public async Task Create(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException("Invalid company");
            }

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
                throw new EntityNotFoundException();

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
        }

        public async Task<Company> Get(int id)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
                throw new EntityNotFoundException();

            return company;
        }

        public async Task<IEnumerable<Company>> GetAll()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task Update(int id, Company entity)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
                throw new EntityNotFoundException();

            company.CompanyName = entity.CompanyName;
            company.CompanyDescription = entity.CompanyDescription;

            await _context.SaveChangesAsync();
        }
    }
}
