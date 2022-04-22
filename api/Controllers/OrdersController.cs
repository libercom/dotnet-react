using Microsoft.AspNetCore.Mvc;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public OrdersController(IOrdersRepository orders, IMapper mapper)
        {
            _orders = orders;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders()
        {
            var orders = await _orders.GetAll();

            return orders.ToList();
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

            return NoContent();
        }
    }
}
