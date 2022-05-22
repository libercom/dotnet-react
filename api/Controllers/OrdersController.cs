using Microsoft.AspNetCore.Mvc;
using core.Repositories.Abstractions;
using core.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class OrdersController : Controller
    {
        private readonly IOrdersRepository _orders;

        public OrdersController(IOrdersRepository orders)
        {
            _orders = orders;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders(
            [FromQuery] int pageNumber, 
            [FromQuery] int pageSize, 
            [FromQuery] string sortCriteria,
            [FromQuery] string sortType,
            [FromQuery] int destinationCountry,
            [FromQuery] int sendingCountry)
        {
            try
            {
                var orders = await _orders.GetAll();
                
                if (!sortCriteria.Equals("none"))
                {
                    if (sortType.Equals("asc"))
                    {
                        if (sortCriteria.Equals("payment"))
                        {
                            orders = orders.OrderBy(x => x.Payment);
                        } else if (sortCriteria.Equals("shipmentDate"))
                        {
                            orders = orders.OrderBy(x => x.ShipmentDate);
                        } else
                        {
                            orders = orders.OrderBy(x => x.ArrivalDate);
                        }
                    } else
                    {
                        if (sortCriteria.Equals("payment"))
                        {
                            orders = orders.OrderByDescending(x => x.Payment);
                        }
                        else if (sortCriteria.Equals("shipmentDate"))
                        {
                            orders = orders.OrderByDescending(x => x.ShipmentDate);
                        }
                        else
                        {
                            orders = orders.OrderByDescending(x => x.ArrivalDate);
                        }
                    }
                }

                if (destinationCountry != 0)
                {
                    orders = orders.Where(x => x.DestinationCountry.CountryId == destinationCountry);
                }

                if (sendingCountry != 0)
                {
                    orders = orders.Where(x => x.SendingCountry.CountryId == sendingCountry);
                }

                return Ok(new
                {
                    count = orders.Count(),
                    orders = orders
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToList()
                });
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
