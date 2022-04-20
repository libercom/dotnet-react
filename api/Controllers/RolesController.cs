using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using core.Context;
using core.Models;
using core.Repositories.Abstractions;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<ActionResult<IEnumerable<Role>>> GetAllRoles()
        {
            var roles = await _roles.GetAll();

            return roles.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id)
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

        }

        [HttpPost]
        public async Task<IActionResult> PostCompany(Role role)
        {
            try
            {
                await _roles.Create(role);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
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

            return NoContent();
        }
    }
}
