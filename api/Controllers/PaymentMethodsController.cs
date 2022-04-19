using core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using core.Context;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentMethodsController : Controller
    {
        private readonly CargoDBContext _context;

        public PaymentMethodsController(CargoDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentMethod>>> GetAllPaymentMethods()
        {
            return await _context.PaymentMethods.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentMethod>> GetPaymentMethod(int id)
        {
            var paymentMethod = await _context.PaymentMethods.FindAsync(id);

            if (paymentMethod == null)
                return NotFound();

            return paymentMethod;
        }

        [HttpPost]
        public async Task<IActionResult> PostPaymentMethod(PaymentMethod paymentMethod)
        {
            if (paymentMethod == null)
                return BadRequest();

            _context.PaymentMethods.Add(paymentMethod);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPaymentMethod), new { id = paymentMethod.PaymentMethodId }, paymentMethod);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentMethod(int id)
        {
            var paymentMethod = await _context.PaymentMethods.FindAsync(id);

            if (paymentMethod == null)
            {
                return NotFound();
            }

            _context.PaymentMethods.Remove(paymentMethod);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
