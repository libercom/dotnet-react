using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using core.Models;
using core.Context;
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
        [Authorize(Roles = "Admin")]
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
                    HttpOnly = true
                });

                return Ok(_mapper.Map<UserDto>(user));
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
            var user = _mapper.Map<User>(userDto);

            try
            {
                await _users.Create(user);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, _mapper.Map<UserDto>(user));
        }

        [HttpGet("user")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var jwt = Request.Cookies["jwt"];

            var token = _jwtService.Validate(jwt);
            int userId = Convert.ToInt32(token.Issuer);

            var user = await _users.Get(userId);

            return Ok(_mapper.Map<UserDto>(user));
        }
    }
}
