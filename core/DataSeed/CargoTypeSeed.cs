using domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.DataSeed
{
    public static class CargoTypeSeed
    {
        public static void SeedCargoType(this ModelBuilder builder)
        {
            builder.Entity<CargoType>().HasData(
                new CargoType
                {
                    CargoTypeId = 1,
                    CargoTypeName = "Refrigerators"
                },
                new CargoType
                {
                    CargoTypeId = 2,
                    CargoTypeName = "Building Materials"
                },
                new CargoType
                {
                    CargoTypeId = 3,
                    CargoTypeName = "Clothes"
                }
            );
        }
    }
}
