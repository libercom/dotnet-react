using AutoMapper;
using domain.Context;
using domain.Models;
using core.Dtos;
using core.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

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

            if (order == null)
            {
                throw new ArgumentNullException("Invalid order");
            }

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
    }
}
