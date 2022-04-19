using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using core.Models;
using core.Context;
using AutoMapper;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly CargoDBContext _context;
        private readonly IMapper _mapper;

        public UsersController(CargoDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            return await _context.Users.Select(u => _mapper.Map<UserDto>(u)).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            return _mapper.Map<UserDto>(user);
        }

        [HttpPost]
        public async Task<IActionResult> PostUser(UserCreationDto userDto)
        {
            if (userDto == null)
                return BadRequest();

            var user = _mapper.Map<User>(userDto);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
