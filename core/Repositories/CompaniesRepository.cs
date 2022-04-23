using domain.Context;
using domain.Models;
using core.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AutoMapper;
using core.Dtos;

namespace core.Repositories
{
    public class CompaniesRepository : ICompaniesRepository
    {
        private readonly CargoDBContext _context;
        private readonly ILogger<CompaniesRepository> _logger;
        private readonly IMapper _mapper;

        public CompaniesRepository(CargoDBContext context, IMapper mapper, ILogger<CompaniesRepository> logger)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CompanyDto>> GetAll()
        {
            try
            {
                return await _context.Companies.Select(c => _mapper.Map<CompanyDto>(c)).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
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

            if (company == null)
            {
                throw new ArgumentNullException("Invalid company");
            }

            _context.Companies.Add(company);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task Delete(int id)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
                throw new EntityNotFoundException();

            _context.Companies.Remove(company);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task Update(int id, CompanyDto entity)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
                throw new EntityNotFoundException();

            company.CompanyName = entity.CompanyName;
            company.CompanyDescription = entity.CompanyDescription;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
