using Microsoft.AspNetCore.Mvc;
using core.Repositories.Abstractions;
using Microsoft.AspNetCore.Authorization;
using core.Dtos;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly IRolesRepository _roles;

        public RolesController(IRolesRepository roles)
        {
            _roles = roles;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetAllRoles()
        {
            try
            {
                var roles = await _roles.GetAll();

                return roles.ToList();
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDto>> GetRole(int id)
        {
            try
            {
                var role = await _roles.Get(id);

                return role;
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
        public async Task<IActionResult> PostCompany(RoleDto role)
        {
            try
            {
                await _roles.Create(role);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return Problem();
            }

            return CreatedAtAction(nameof(GetRole), new { id = role.RoleId }, role);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            try
            {
                await _roles.Delete(id);
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
    }
}
