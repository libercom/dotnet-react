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
    public class CountriesRepository : ICountriesRepository
    {
        private readonly CargoDBContext _context;
        private readonly IMapper _mapper;

        public CountriesRepository(CargoDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CountryDto>> GetAll()
        {
            return await _context.Countries.Select(c => _mapper.Map<CountryDto>(c)).ToListAsync();
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

            _context.Countries.Add(country);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var country = await _context.Countries.FindAsync(id);

            if (country == null)
                throw new EntityNotFoundException();

            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, CountryDto entity)
        {
            var country = await _context.Countries.FindAsync(id);

            if (country == null)
                throw new EntityNotFoundException();

            country.CountryName = entity.CountryName;

            await _context.SaveChangesAsync();
        }
    }
}
