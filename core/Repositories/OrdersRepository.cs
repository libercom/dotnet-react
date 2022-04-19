using core.Context;
using core.Models;
using core.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly CargoDBContext _context;

        public OrdersRepository(CargoDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.PaymentMethod)
                .Include(o => o.CargoType)
                .Include(o => o.SendingCountry)
                .Include(o => o.DestinationCountry)
                .ToListAsync();
        }

        public async Task<Order> Get(int id)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.PaymentMethod)
                .Include(o => o.CargoType)
                .Include(o => o.SendingCountry)
                .Include(o => o.DestinationCountry)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (order == null)
                throw new EntityNotFoundException();

            return order;
        }

        public async Task Create(Order order)
        {
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

        public async Task Update(int id, Order entity)
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
