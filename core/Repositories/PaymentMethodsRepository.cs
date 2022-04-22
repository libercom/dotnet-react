using domain.Context;
using domain.Models;
using core.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Repositories
{
    public class PaymentMethodsRepository : IPaymentMethodsRepository
    {
        private readonly CargoDBContext _context;

        public PaymentMethodsRepository(CargoDBContext context)
        {
            _context = context;
        }

        public async Task Create(PaymentMethod paymentMethod)
        {
            if (paymentMethod == null)
            {
                throw new ArgumentNullException("Invalid payment method");
            }

            _context.PaymentMethods.Add(paymentMethod);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var paymentMethod = await _context.PaymentMethods.FindAsync(id);

            if (paymentMethod == null)
                throw new EntityNotFoundException();

            _context.PaymentMethods.Remove(paymentMethod);
            await _context.SaveChangesAsync();
        }

        public async Task<PaymentMethod> Get(int id)
        {
            var paymentMethod = await _context.PaymentMethods.FindAsync(id);

            if (paymentMethod == null)
                throw new EntityNotFoundException();

            return paymentMethod;
        }

        public async Task<IEnumerable<PaymentMethod>> GetAll()
        {
            return await _context.PaymentMethods.ToListAsync();
        }

        public async Task Update(int id, PaymentMethod entity)
        {
            var paymentMethod = await _context.PaymentMethods.FindAsync(id);

            if (paymentMethod == null)
                throw new EntityNotFoundException();

            paymentMethod.PaymentMethodName = entity.PaymentMethodName;

            await _context.SaveChangesAsync();
        }
    }
}
