using domain.Models;
using Microsoft.AspNetCore.Mvc;
using core.Repositories.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class PaymentMethodsController : Controller
    {
        private readonly IPaymentMethodsRepository _paymentMethods;

        public PaymentMethodsController(IPaymentMethodsRepository paymentMethods)
        {
            _paymentMethods = paymentMethods;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentMethod>>> GetAllPaymentMethods()
        {
            var paymentMethods = await _paymentMethods.GetAll();

            return paymentMethods.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentMethod>> GetPaymentMethod(int id)
        {
            try
            {
                var paymentMethod = await _paymentMethods.Get(id);

                return paymentMethod;
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }

        }

        [HttpPost]
        public async Task<IActionResult> PostPaymentMethod(PaymentMethod paymentMethod)
        {
            try
            {
                await _paymentMethods.Create(paymentMethod);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetPaymentMethod), new { id = paymentMethod.PaymentMethodId }, paymentMethod);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentMethod(int id)
        {
            try
            {
                await _paymentMethods.Delete(id);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
