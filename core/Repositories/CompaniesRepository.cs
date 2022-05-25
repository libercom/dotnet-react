using core.Context;
using domain.Models;
using core.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AutoMapper;
using common.Dtos;
using common.Exceptions;

namespace core.Repositories
{
    public class CompaniesRepository : ICompaniesRepository
    {
        private readonly CargoDBContext _context;
        private readonly IMapper _mapper;

        public CompaniesRepository(CargoDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CompanyDto>> GetAll()
        {
            return await _context.Companies.Select(c => _mapper.Map<CompanyDto>(c)).ToListAsync();
        }

        public async Task<CompanyDto> Get(int id)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
                throw new EntityNotFoundException();

            return _mapper.Map<CompanyDto>(company);
        }

        public async Task Create(CompanyDto companyDto)
        {
            var company = _mapper.Map<Company>(companyDto);

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

        public async Task Update(int id, CompanyDto entity)
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
