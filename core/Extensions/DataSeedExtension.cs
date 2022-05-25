using core.DataSeed;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Extensions
{
    public static class DataSeedExtension
    {
        public static void Seed(this ModelBuilder builder)
        {
            builder.SeedRole();
            builder.SeedPaymentMethod();
            builder.SeedCompany();
            builder.SeedCountry();
            builder.SeedCargoType();
            builder.SeedUser();
        }
    }
}
