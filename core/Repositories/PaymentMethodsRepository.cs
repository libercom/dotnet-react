using core.Context;
using domain.Models;
using core.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AutoMapper;
using common.Dtos;
using common.Exceptions;

namespace core.Repositories
{
    public class PaymentMethodsRepository : IPaymentMethodsRepository
    {
        private readonly CargoDBContext _context;
        private readonly IMapper _mapper;

        public PaymentMethodsRepository(CargoDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PaymentMethodDto>> GetAll()
        {
            return await _context.PaymentMethods.Select(p => _mapper.Map<PaymentMethodDto>(p)).ToListAsync();
        }

        public async Task<PaymentMethodDto> Get(int id)
        {
            var paymentMethod = await _context.PaymentMethods.FindAsync(id);

            if (paymentMethod == null)
                throw new EntityNotFoundException();

            return _mapper.Map<PaymentMethodDto>(paymentMethod);
        }

        public async Task Create(PaymentMethodDto paymentMethodDto)
        {
            var paymentMethod = _mapper.Map<PaymentMethod>(paymentMethodDto);

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

        public async Task Update(int id, PaymentMethodDto entity)
        {
            var paymentMethod = await _context.PaymentMethods.FindAsync(id);

            if (paymentMethod == null)
                throw new EntityNotFoundException();

            paymentMethod.PaymentMethodName = entity.PaymentMethodName;

            await _context.SaveChangesAsync();
        }
    }
}
