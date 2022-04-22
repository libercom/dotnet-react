using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using core.Repositories.Abstractions;
using api.Services.Abstractions;
using core.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUsersRepository _users;
        private readonly IJwtTokenService _jwtService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public UsersController(
            IUsersRepository users,
            IMapper mapper,
            IJwtTokenService jwtService,
            IAuthenticationService authenticationService
        )
        {
            _users = users;
            _mapper = mapper;
            _jwtService = jwtService;
            _authenticationService = authenticationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _users.GetAll();

            return users.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            try
            {
                var user = await _users.Get(id);

                return user;

            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostUser(UserCreationDto userDto)
        {
            try
            {
                await _users.Create(userDto);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetUser), new { id = userDto.UserId }, userDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
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

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate(UserLoginDto userDto)
        {
            try
            {
                var user = await _authenticationService.Authenticate(userDto);

                var jwt = _jwtService.Generate(user.UserId, user.Role.RoleType);

                Response.Cookies.Append("jwt", jwt, new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddHours(1)
                });

                return Ok("You logged in");
            }
            catch (Exception ex) when (
                ex is InvalidCredentialsException
                || ex is EntityNotFoundException
            )
            {
                return BadRequest("Invalid Credentials");
            }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserCreationDto userDto)
        {
            try
            {
                await _users.Create(userDto);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetUser), new { id = userDto.UserId }, userDto);
        }

        [HttpGet("user")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var jwt = Request.Cookies["jwt"];

            var token = _jwtService.Validate(jwt);
            int userId = Convert.ToInt32(token.Issuer);

            var user = await _users.Get(userId);

            return user;
        }
    }
}
