using domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.DataSeed
{
    public static class CountrySeed
    {
        public static void SeedCountry(this ModelBuilder builder)
        {
            builder.Entity<Country>().HasData(
                new Country
                {
                    CountryId = 1,
                    CountryName = "Moldova"
                },
                new Country
                {
                    CountryId = 2,
                    CountryName = "Romania",
                },
                new Country
                {
                    CountryId = 3,
                    CountryName = "Bulgaria"
                },
                new Country
                {
                    CountryId = 4,
                    CountryName = "Turkey"
                }
            );
        }
    }
}
