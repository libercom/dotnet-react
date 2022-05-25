using Microsoft.AspNetCore.Mvc;
using core.Repositories.Abstractions;
using Microsoft.AspNetCore.Authorization;
using common.Dtos;

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
            var roles = await _roles.GetAll();

            return roles.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDto>> GetRole(int id)
        {
            var role = await _roles.Get(id);

            return role;
        }

        [HttpPost]
        public async Task<IActionResult> PostCompany(RoleDto role)
        {
            await _roles.Create(role);

            return CreatedAtAction(nameof(GetRole), new { id = role.RoleId }, role);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            await _roles.Delete(id);

            return NoContent();
        }
    }
}
