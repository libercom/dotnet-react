using AutoMapper;
using domain.Context;
using domain.Models;
using core.Dtos;
using core.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace core.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly CargoDBContext _context;
        private readonly ILogger<UsersRepository> _logger;
        private readonly IMapper _mapper;

        public UsersRepository(CargoDBContext context, IMapper mapper, ILogger<UsersRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<UserDto>> GetAll()
        {
            try
            {
                var users = await _context.Users
                    .Include(u => u.Company)
                    .Include(u => u.Role)
                    .Include(u => u.Country)
                    .ToListAsync();

                return users.Select(u => _mapper.Map<UserDto>(u));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<UserDto> Get(int id)
        {
            try
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
            catch (EntityNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task Create(UserCreationDto userDto)
        {
            var user = _mapper.Map<User>(userDto);

            if (user == null)
            {
                throw new ArgumentNullException("Invalid user");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _context.Users.Add(user);

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
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                throw new EntityNotFoundException();

            _context.Users.Remove(user);

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

        public async Task<UserDto> GetByEmail(string email)
        {
            try
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
            catch (EntityNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
