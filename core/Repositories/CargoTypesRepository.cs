using domain.Context;
using domain.Models;
using core.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AutoMapper;
using core.Dtos;

namespace core.Repositories
{
    public class CargoTypesRepository : ICargoTypesRepository
    {
        private readonly CargoDBContext _context;
        private readonly ILogger<CargoTypesRepository> _logger;
        private readonly IMapper _mapper;

        public CargoTypesRepository(CargoDBContext context, IMapper mapper, ILogger<CargoTypesRepository> logger)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CargoTypeDto>> GetAll()
        {
            try
            {
                return await _context.CargoTypes.Select(c => _mapper.Map<CargoTypeDto>(c)).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
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

            if (cargoType == null)
            {
                throw new ArgumentNullException("Invalid cargo type");
            }

            _context.CargoTypes.Add(cargoType);

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
            var cargoType = await _context.CargoTypes.FindAsync(id);

            if (cargoType == null)
                throw new EntityNotFoundException();

            _context.CargoTypes.Remove(cargoType);

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

        public async Task Update(int id, CargoTypeDto entity)
        {
            var cargoType = await _context.CargoTypes.FindAsync(id);

            if (cargoType == null)
                throw new EntityNotFoundException();

            cargoType.CargoTypeName = entity.CargoTypeName;

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
