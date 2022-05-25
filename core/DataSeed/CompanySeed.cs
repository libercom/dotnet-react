using domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.DataSeed
{
    public static class CompanySeed
    {
        public static void SeedCompany(this ModelBuilder builder)
        {
            builder.Entity<Company>().HasData(
                new Company
                {
                    CompanyId = 1,
                    CompanyName = "GeoTrans",
                    CompanyDescription = "Doing intersting stuff"
                },
                new Company
                {
                    CompanyId = 2,
                    CompanyName = "NeoTrans",
                    CompanyDescription = "Transporting company"
                }
            );
        }
    }
}
