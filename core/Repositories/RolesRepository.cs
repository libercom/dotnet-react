using domain.Context;
using domain.Models;
using core.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AutoMapper;
using core.Dtos;

namespace core.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private readonly CargoDBContext _context;
        private readonly ILogger<RolesRepository> _logger;
        private readonly IMapper _mapper;

        public RolesRepository(CargoDBContext context, IMapper mapper, ILogger<RolesRepository> logger)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleDto>> GetAll()
        {
            try
            {
                return await _context.Roles.Select(r => _mapper.Map<RoleDto>(r)).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<RoleDto> Get(int id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
                throw new EntityNotFoundException();

            return _mapper.Map<RoleDto>(role);
        }

        public async Task Create(RoleDto roleDto)
        {
            var role = _mapper.Map<Role>(roleDto);

            if (role == null)
            {
                throw new ArgumentNullException("Invalid role");
            }

            _context.Roles.Add(role);

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
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
                throw new EntityNotFoundException();

            _context.Roles.Remove(role);

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

        public async Task Update(int id, RoleDto entity)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
                throw new EntityNotFoundException();

            role.RoleType = entity.RoleType;

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
