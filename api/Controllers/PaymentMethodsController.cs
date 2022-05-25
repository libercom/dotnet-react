using Microsoft.AspNetCore.Mvc;
using core.Repositories.Abstractions;
using Microsoft.AspNetCore.Authorization;
using common.Dtos;

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
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PaymentMethodDto>>> GetAllPaymentMethods()
        {
            var paymentMethods = await _paymentMethods.GetAll();

            return paymentMethods.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentMethodDto>> GetPaymentMethod(int id)
        {
            var paymentMethod = await _paymentMethods.Get(id);

            return paymentMethod;
        }

        [HttpPost]
        public async Task<IActionResult> PostPaymentMethod(PaymentMethodDto paymentMethod)
        {
            await _paymentMethods.Create(paymentMethod);

            return CreatedAtAction(nameof(GetPaymentMethod), new { id = paymentMethod.PaymentMethodId }, paymentMethod);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentMethod(int id)
        {
            await _paymentMethods.Delete(id);

            return NoContent();
        }
    }
}
