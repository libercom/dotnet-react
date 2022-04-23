using domain.Context;
using domain.Models;
using core.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AutoMapper;
using core.Dtos;

namespace core.Repositories
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly CargoDBContext _context;
        private readonly ILogger<CountriesRepository> _logger;
        private readonly IMapper _mapper;

        public CountriesRepository(CargoDBContext context, IMapper mapper, ILogger<CountriesRepository> logger)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CountryDto>> GetAll()
        {
            try
            {
                return await _context.Countries.Select(c => _mapper.Map<CountryDto>(c)).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<CountryDto> Get(int id)
        {
            var country = await _context.Countries.FindAsync(id);

            if (country == null)
                throw new EntityNotFoundException();

            return _mapper.Map<CountryDto>(country);
        }

        public async Task Create(CountryDto countryDto)
        {
            var country = _mapper.Map<Country>(countryDto);

            if (country == null)
            {
                throw new ArgumentNullException("Invalid country");
            }

            _context.Countries.Add(country);

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
            var country = await _context.Countries.FindAsync(id);

            if (country == null)
                throw new EntityNotFoundException();

            _context.Countries.Remove(country);

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

        public async Task Update(int id, CountryDto entity)
        {
            var country = await _context.Countries.FindAsync(id);

            if (country == null)
                throw new EntityNotFoundException();

            country.CountryName = entity.CountryName;

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
