using AutoMapper;
using core.Context;
using domain.Models;
using common.Dtos;
using core.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using common.Exceptions;

namespace core.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly CargoDBContext _context;
        private readonly IMapper _mapper;

        public UsersRepository(CargoDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetAll()
        {
            var users = await _context.Users
                .Include(u => u.Company)
                .Include(u => u.Role)
                .Include(u => u.Country)
                .ToListAsync();

            return users.Select(u => _mapper.Map<UserDto>(u));
        }

        public async Task<UserDto> Get(int id)
        {
            var user = await _context.Users
                .Include(u => u.Company)
                .Include(u => u.Role)
                .Include(u => u.Country)
                .FirstOrDefaultAsync(u => u.UserId == id);           

            if (user == null)
                throw new EntityNotFoundException();

            return _mapper.Map<UserDto>(user);
        }

        public async Task Create(UserCreationDto userDto)
        {
            var user = _mapper.Map<User>(userDto);

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

        public async Task Update(int id, UserCreationDto entity)
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

        public async Task<UserDto> GetByEmail(string email)
        {
            var user = await _context.Users
                .Include(u => u.Company)
                .Include(u => u.Role)
                .Include(u => u.Country)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                throw new EntityNotFoundException();

            return _mapper.Map<UserDto>(user);
        }
    }
}
