using core.Dtos;

namespace core.Repositories.Abstractions
{
    public interface IUsersRepository : IRepository<UserDto, UserCreationDto>
    {
        public Task<UserDto> GetByEmail(string email);
    }
}
