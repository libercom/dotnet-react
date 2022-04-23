using Microsoft.AspNetCore.Mvc;
using core.Repositories.Abstractions;
using core.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrdersRepository _orders;

        public OrdersController(IOrdersRepository orders)
        {
            _orders = orders;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders()
        {
            try
            {
                var orders = await _orders.GetAll();

                return orders.ToList();
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            try
            {
                var order = await _orders.Get(id);
                
                return order;
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
        public async Task<IActionResult> PostOrder(OrderCreationDto orderDto)
        {
            try
            {
                await _orders.Create(orderDto);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();

            }
            catch (Exception)
            {
                return Problem();
            }

            return CreatedAtAction(nameof(GetOrder), new { id = orderDto.OrderId }, orderDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                await _orders.Delete(id);
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
