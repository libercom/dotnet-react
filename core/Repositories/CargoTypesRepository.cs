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
    public class CargoTypesRepository : ICargoTypesRepository
    {
        private readonly CargoDBContext _context;
        private readonly IMapper _mapper;

        public CargoTypesRepository(CargoDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CargoTypeDto>> GetAll()
        {
            return await _context.CargoTypes.Select(c => _mapper.Map<CargoTypeDto>(c)).ToListAsync();
        }

        public async Task<CargoTypeDto> Get(int id)
        {
            var cargoType = await _context.CargoTypes.FindAsync(id);

            if (cargoType == null)
                throw new EntityNotFoundException();

            return _mapper.Map<CargoTypeDto>(cargoType);
        }

        public async Task Create(CargoTypeDto cargoTypeDto)
        {
            var cargoType = _mapper.Map<CargoType>(cargoTypeDto);

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

        public async Task Update(int id, CargoTypeDto entity)
        {
            var cargoType = await _context.CargoTypes.FindAsync(id);

            if (cargoType == null)
                throw new EntityNotFoundException();

            cargoType.CargoTypeName = entity.CargoTypeName;

            await _context.SaveChangesAsync();
        }
    }
}
