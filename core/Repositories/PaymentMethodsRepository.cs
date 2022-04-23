using domain.Context;
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
using core.Dtos;

namespace core.Repositories
{
    public class PaymentMethodsRepository : IPaymentMethodsRepository
    {
        private readonly CargoDBContext _context;
        private readonly ILogger<PaymentMethodsRepository> _logger;
        private readonly IMapper _mapper;

        public PaymentMethodsRepository(CargoDBContext context, IMapper mapper, ILogger<PaymentMethodsRepository> logger)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PaymentMethodDto>> GetAll()
        {
            try
            {
                return await _context.PaymentMethods.Select(p => _mapper.Map<PaymentMethodDto>(p)).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
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
            if (paymentMethod == null)
            {
                throw new ArgumentNullException("Invalid payment method");
            }

            _context.PaymentMethods.Add(paymentMethod);

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
            var paymentMethod = await _context.PaymentMethods.FindAsync(id);

            if (paymentMethod == null)
                throw new EntityNotFoundException();

            _context.PaymentMethods.Remove(paymentMethod);

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

        public async Task Update(int id, PaymentMethodDto entity)
        {
            var paymentMethod = await _context.PaymentMethods.FindAsync(id);

            if (paymentMethod == null)
                throw new EntityNotFoundException();

            paymentMethod.PaymentMethodName = entity.PaymentMethodName;

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
