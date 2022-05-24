using Microsoft.AspNetCore.Mvc;
using core.Repositories.Abstractions;
using core.Dtos;
using Microsoft.AspNetCore.Authorization;
using api.Services.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using core.Models;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class OrdersController : Controller
    {
        private readonly IOrdersRepository _orders;
        private readonly IJwtTokenService _jwtService;

        public OrdersController(IOrdersRepository orders, IJwtTokenService jwtService)
        {
            _orders = orders;
            _jwtService = jwtService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse>> GetAllOrders(
            [FromQuery] int pageNumber, 
            [FromQuery] int pageSize, 
            [FromQuery] string sortCriteria,
            [FromQuery] string sortType,
            [FromQuery] int destinationCountry,
            [FromQuery] int sendingCountry,
            [FromQuery] bool onlyMyOrders)
        {
            int userId = -1;

            if (onlyMyOrders)
            {
                var jwt = Request.Cookies["jwt"];

                try
                {
                    userId = _jwtService.GetIssuer(jwt);
                }
                catch (Exception)
                {
                    return BadRequest("Invalid token");
                }
            }

            try
            {
                PagedRequest pagedRequest = new PagedRequest
                {
                    PageSize = pageSize,
                    PageNumber = pageNumber,
                    SendingCountry = sendingCountry,
                    DestinationCountry = destinationCountry,
                    UserId = userId,
                    SortCriteria = sortCriteria,
                    SortType = sortType
                };

                var orders = await _orders.GetPagedData(pagedRequest);

                return Ok(orders);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, OrderCreationDto orderDto)
        {
            try
            {
                await _orders.Update(id, orderDto);
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
