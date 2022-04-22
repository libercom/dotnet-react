using domain.Context;
using domain.Models;
using core.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace core.Repositories
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly CargoDBContext _context;

        public CountriesRepository(CargoDBContext context)
        {
            _context = context;
        }

        public async Task Create(Country country)
        {
            if (country == null)
            {
                throw new ArgumentNullException("Invalid country");
            }

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

        public async Task<Country> Get(int id)
        {
            var country = await _context.Countries.FindAsync(id);

            if (country == null)
                throw new EntityNotFoundException();

            return country;
        }

        public async Task<IEnumerable<Country>> GetAll()
        {
            return await _context.Countries.ToListAsync();
        }

        public async Task Update(int id, Country entity)
        {
            var country = await _context.Countries.FindAsync(id);

            if (country == null)
                throw new EntityNotFoundException();

            country.CountryName = entity.CountryName;

            await _context.SaveChangesAsync();
        }
    }
}
