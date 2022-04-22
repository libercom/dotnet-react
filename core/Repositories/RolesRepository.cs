using domain.Context;
using domain.Models;
using core.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace core.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private readonly CargoDBContext _context;

        public RolesRepository(CargoDBContext context)
        {
            _context = context;
        }

        public async Task Create(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("Invalid role");
            }

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

        public async Task<Role> Get(int id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
                throw new EntityNotFoundException();

            return role;
        }

        public async Task<IEnumerable<Role>> GetAll()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task Update(int id, Role entity)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
                throw new EntityNotFoundException();

            role.RoleType = entity.RoleType;

            await _context.SaveChangesAsync();
        }
    }
}
