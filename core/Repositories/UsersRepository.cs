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
    public class UsersRepository : IUsersRepository
    {
        private readonly CargoDBContext _context;

        public UsersRepository(CargoDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users
                .Include(u => u.Company)
                .Include(u => u.Role)
                .Include(u => u.Country)
                .ToListAsync();
        }

        public async Task<User> Get(int id)
        {
            var user = await _context.Users
                .Include(u => u.Company)
                .Include(u => u.Role)
                .Include(u => u.Country)
                .FirstOrDefaultAsync(u => u.UserId == id);           

            if (user == null)
                throw new EntityNotFoundException();

            return user;
        }

        public async Task Create(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("Invalid user");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                throw new EntityNotFoundException();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, User entity)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                throw new EntityNotFoundException();

            user.FirstName = entity.FirstName;
            user.LastName = entity.LastName;
            user.Email = entity.Email;
            user.PhoneNumber = entity.PhoneNumber;
            user.RoleId = entity.RoleId;
            user.CompanyId = entity.CompanyId;
            user.CountryId = entity.CountryId;

            await _context.SaveChangesAsync();
        }
    }
}
