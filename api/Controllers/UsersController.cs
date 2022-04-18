using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Context;
using Project.Models;
using Project.Services;

namespace Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly CargoDBContext _context;
        private readonly IDtoMappingService _dtoMapper;

        public UsersController(CargoDBContext context, IDtoMappingService dtoMapper)
        {
            _context = context;
            _dtoMapper = dtoMapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            return await _context.Users.Select(u => _dtoMapper.UserToDto(u)).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            return _dtoMapper.UserToDto(user);
        }

        [HttpPost]
        public async Task<IActionResult> PostUser(UserCreationDto userDto)
        {
            if (userDto == null)
                return BadRequest();

            var user = _dtoMapper.DtoToUser(userDto);

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
