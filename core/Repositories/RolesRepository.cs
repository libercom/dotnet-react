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
    public class RolesRepository : IRolesRepository
    {
        private readonly CargoDBContext _context;
        private readonly IMapper _mapper;

        public RolesRepository(CargoDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleDto>> GetAll()
        {
            return await _context.Roles.Select(r => _mapper.Map<RoleDto>(r)).ToListAsync();
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

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
                throw new EntityNotFoundException();

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, RoleDto entity)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
                throw new EntityNotFoundException();

            role.RoleType = entity.RoleType;

            await _context.SaveChangesAsync();
        }
    }
}
