using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using core.Models;
using core.Context;
using AutoMapper;
using core.Repositories.Abstractions;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUsersRepository _users;
        private readonly IMapper _mapper;

        public UsersController(IUsersRepository users, IMapper mapper)
        {
            _users = users;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _users.GetAll();
            
            return users.Select(u => _mapper.Map<UserDto>(u)).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            try
            {
                var user = await _users.Get(id);
                
                return _mapper.Map<UserDto>(user);

            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostUser(UserCreationDto userDto)
        {
            var user = _mapper.Map<User>(userDto);

            try
            {
                await _users.Create(user);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _users.Delete(id);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
