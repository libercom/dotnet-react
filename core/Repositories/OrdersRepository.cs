using AutoMapper;
using domain.Context;
using domain.Models;
using core.Dtos;
using core.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace core.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly CargoDBContext _context;
        private readonly ILogger<OrdersRepository> _logger;
        private readonly IMapper _mapper;

        public OrdersRepository(CargoDBContext context, IMapper mapper, ILogger<OrdersRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<OrderDto>> GetAll()
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<OrderDto> Get(int id)
        {
            try
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
            catch (EntityNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task Create(OrderCreationDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto); ;

            if (order == null)
            {
                throw new ArgumentNullException("Invalid order");
            }

            _context.Orders.Add(order);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task Delete(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                throw new EntityNotFoundException();

            _context.Orders.Remove(order);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
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

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
