using AutoMapper;
using core.Context;
using domain.Models;
using common.Dtos;
using core.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using common.Models;
using common.Exceptions;

namespace core.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly CargoDBContext _context;
        private readonly IMapper _mapper;

        public OrdersRepository(CargoDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDto>> GetAll()
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                    .ThenInclude(u => u.Role)
                .Include(o => o.User)
                    .ThenInclude(u => u.Company)
                .Include(o => o.User)
                    .ThenInclude(u => u.Country)
                .Include(o => o.PaymentMethod)
                .Include(o => o.CargoType)
                .Include(o => o.SendingCountry)
                .Include(o => o.DestinationCountry)
                .ToListAsync();

            return orders.Select(o => _mapper.Map<OrderDto>(o));
        }

        public async Task<OrderDto> Get(int id)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                    .ThenInclude(u => u.Role)
                .Include(o => o.User)
                    .ThenInclude(u => u.Company)
                .Include(o => o.User)
                    .ThenInclude(u => u.Country)
                .Include(o => o.PaymentMethod)
                .Include(o => o.CargoType)
                .Include(o => o.SendingCountry)
                .Include(o => o.DestinationCountry)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (order == null)
                throw new EntityNotFoundException();

            return _mapper.Map<OrderDto>(order);
        }

        public async Task Create(OrderCreationDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto); ;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                throw new EntityNotFoundException();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, OrderCreationDto entity)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                throw new EntityNotFoundException();

            order.UserId = entity.UserId;
            order.ShipmentDate = entity.ShipmentDate;
            order.ArrivalDate = entity.ArrivalDate;
            order.Payment = entity.Payment;
            order.SendingCountryId = entity.SendingCountryId;
            order.DestinationCountryId = entity.DestinationCountryId;
            order.PaymentMethodId = entity.PaymentMethodId;
            order.CargoTypeId = entity.CargoTypeId;

            await _context.SaveChangesAsync();
        }

        public async Task<PagedResponse> GetPagedData(PagedRequest request)
        {
            IQueryable<Order> orders = _context.Orders
                .Include(o => o.User)
                    .ThenInclude(u => u.Role)
                .Include(o => o.User)
                    .ThenInclude(u => u.Company)
                .Include(o => o.User)
                    .ThenInclude(u => u.Country)
                .Include(o => o.PaymentMethod)
                .Include(o => o.CargoType)
                .Include(o => o.SendingCountry)
                .Include(o => o.DestinationCountry);

            if (!request.SortCriteria.Equals("none"))
            {
                if (request.SortType.Equals("asc"))
                {
                    if (request.SortCriteria.Equals("payment"))
                    {
                        orders = orders.OrderBy(x => x.Payment);
                    }
                    else if (request.SortCriteria.Equals("shipmentDate"))
                    {
                        orders = orders.OrderBy(x => x.ShipmentDate);
                    }
                    else
                    {
                        orders = orders.OrderBy(x => x.ArrivalDate);
                    }
                }
                else
                {
                    if (request.SortCriteria.Equals("payment"))
                    {
                        orders = orders.OrderByDescending(x => x.Payment);
                    }
                    else if (request.SortCriteria.Equals("shipmentDate"))
                    {
                        orders = orders.OrderByDescending(x => x.ShipmentDate);
                    }
                    else
                    {
                        orders = orders.OrderByDescending(x => x.ArrivalDate);
                    }
                }
            }

            if (request.DestinationCountry != 0)
            {
                orders = orders.Where(x => x.DestinationCountry.CountryId == request.DestinationCountry);
            }

            if (request.SendingCountry != 0)
            {
                orders = orders.Where(x => x.SendingCountry.CountryId == request.SendingCountry);
            }

            if (request.UserId != -1)
            {
                orders = orders.Where(x => x.User.UserId == request.UserId);
            }

            var filteredOrders = (await orders.ToListAsync()).Select(o => _mapper.Map<OrderDto>(o));

            return new PagedResponse
            {
                Count = filteredOrders.Count(),
                Orders = filteredOrders
                            .Skip((request.PageNumber - 1) * request.PageSize)
                            .Take(request.PageSize)
                            .ToList()
            };
        }
    }
}
