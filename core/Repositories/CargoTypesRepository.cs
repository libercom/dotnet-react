using core.Context;
using core.Models;
using core.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Repositories
{
    public class CargoTypesRepository : ICargoTypesRepository
    {
        private readonly CargoDBContext _context;

        public CargoTypesRepository(CargoDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CargoType>> GetAll()
        {
            return await _context.CargoTypes.ToListAsync();
        }

        public async Task<CargoType> Get(int id)
        {
            var cargoType = await _context.CargoTypes.FindAsync(id);

            if (cargoType == null)
                throw new EntityNotFoundException();

            return cargoType;
        }

        public async Task Create(CargoType cargoType)
        {
            if (cargoType == null)
            {
                throw new ArgumentNullException("Invalid cargo type");
            }

            _context.CargoTypes.Add(cargoType);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var cargoType = await _context.CargoTypes.FindAsync(id);

            if (cargoType == null)
                throw new EntityNotFoundException();

            _context.CargoTypes.Remove(cargoType);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, CargoType entity)
        {
            var cargoType = await _context.CargoTypes.FindAsync(id);

            if (cargoType == null)
                throw new EntityNotFoundException();

            cargoType.CargoTypeName = entity.CargoTypeName;

            await _context.SaveChangesAsync();
        }
    }
}
