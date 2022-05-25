using domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.DataSeed
{
    public static class PaymentMethodSeed
    {
        public static void SeedPaymentMethod(this ModelBuilder builder)
        {
            builder.Entity<PaymentMethod>().HasData(
                new PaymentMethod
                {
                    PaymentMethodId = 1,
                    PaymentMethodName = "Transfer"
                },
                new PaymentMethod
                {
                    PaymentMethodId = 2,
                    PaymentMethodName = "Cash"
                }
            );
        }
    }
}
