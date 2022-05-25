using Microsoft.AspNetCore.Mvc;
using core.Repositories.Abstractions;
using api.Services.Abstractions;
using common.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Cors;
using common.Models;
using api.Infrastructure.AppSettings;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class UsersController : Controller
    {
        private readonly int _cookieDuration;
        private readonly IUsersRepository _users;
        private readonly IJwtTokenService _jwtService;
        private readonly IAuthenticationService _authenticationService;

        public UsersController(
            IUsersRepository users,
            IJwtTokenService jwtService,
            IAuthenticationService authenticationService,
            IConfiguration configuration
        )
        {
            _users = users;
            _jwtService = jwtService;
            _authenticationService = authenticationService;
            _cookieDuration = configuration.GetSection("AppSettings").Get<AppSettings>().Duration;
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
            var user = await _users.Get(id);

            return user;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostUser(UserCreationDto userDto)
        {
            userDto.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            await _users.Create(userDto);

            return CreatedAtAction(nameof(GetUser), new { id = userDto.UserId }, userDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _users.Delete(id);

            return NoContent();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> Authenticate(UserLoginRequest userDto)
        {
            var user = await _authenticationService.Authenticate(userDto);

            var jwt = _jwtService.Generate(user.UserId, user.Role.RoleType);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddHours(_cookieDuration)
            });

            return user;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserCreationDto userDto)
        {
            await _users.Create(userDto);

            return CreatedAtAction(nameof(GetUser), new { id = userDto.UserId }, userDto);
        }

        [HttpGet("user")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var jwt = Request.Cookies["jwt"];
            var userId = _jwtService.GetIssuer(jwt);
            var user = await _users.Get(userId);
                
            return user;
        }

        [AllowAnonymous]
        [HttpGet("check/{email}")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            await _users.GetByEmail(email);

            return true;
        }

        [AllowAnonymous]
        [HttpGet("logout")]
        public void Logout()
        {
            if (Request.Cookies["jwt"] != null)
            {
                Response.Cookies.Append("jwt", "", new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddHours(-1)
                });
            }
        }
    }
}
