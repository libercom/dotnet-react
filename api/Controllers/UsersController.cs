using Microsoft.AspNetCore.Mvc;
using core.Repositories.Abstractions;
using api.Services.Abstractions;
using core.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Cors;

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

        public UsersController(
            IUsersRepository users,
            IJwtTokenService jwtService,
            IAuthenticationService authenticationService
        )
        {
            _users = users;
            _jwtService = jwtService;
            _authenticationService = authenticationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            try
            {
                var users = await _users.GetAll();
            
                return users.ToList();
            }
            catch (Exception)
            {
                return Problem();
            }
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
            catch (Exception)
            {
                return Problem();
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
            catch (Exception)
            {
                return Problem();
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
            catch (Exception)
            {
                return Problem();
            }

            return NoContent();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> Authenticate(UserLoginDto userDto)
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

                return user;
            }
            catch (Exception ex) when (
                ex is InvalidCredentialsException
                || ex is EntityNotFoundException
            )
            {
                return BadRequest("Invalid Credentials");
            }
            catch (Exception)
            {
                return Problem();
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
            catch (Exception)
            {
                return Problem();
            }

            return CreatedAtAction(nameof(GetUser), new { id = userDto.UserId }, userDto);
        }

        [HttpGet("user")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var jwt = Request.Cookies["jwt"];
            JwtSecurityToken token;

            try
            {
                token = _jwtService.Validate(jwt);
            }
            catch (Exception)
            {
                return BadRequest("Invalid token");
            }

            try
            {
                int userId = Convert.ToInt32(token.Issuer);
                var user = await _users.Get(userId);
                
                return user;
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        [AllowAnonymous]
        [HttpGet("check/{email}")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            try
            {
                await _users.GetByEmail(email);

                return true;
            }
            catch (EntityNotFoundException)
            {
                return false;
            }
            catch (Exception)
            {
                return Problem();
            }
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
