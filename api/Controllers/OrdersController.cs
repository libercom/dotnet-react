using Microsoft.AspNetCore.Mvc;
using core.Repositories.Abstractions;
using common.Dtos;
using Microsoft.AspNetCore.Authorization;
using api.Services.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using common.Models;

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

                userId = _jwtService.GetIssuer(jwt);
            }

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

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var order = await _orders.Get(id);
                
            return order;
        }

        [HttpPost]
        public async Task<IActionResult> PostOrder(OrderCreationDto orderDto)
        {
            await _orders.Create(orderDto);

            return CreatedAtAction(nameof(GetOrder), new { id = orderDto.OrderId }, orderDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orders.Delete(id);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, OrderCreationDto orderDto)
        {
            await _orders.Update(id, orderDto);

            return NoContent();
        }
    }
}
