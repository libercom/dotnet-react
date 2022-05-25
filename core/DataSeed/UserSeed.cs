using domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.DataSeed
{
    public static class UserSeed
    {
        public static void SeedUser(this ModelBuilder builder)
        {
            builder.Entity<User>().HasData(
                new User {
                    UserId = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@gmail.com",
                    PhoneNumber = "+37368742312",
                    Password = BCrypt.Net.BCrypt.HashPassword("z1234567"),
                    RoleId = 1,
                    CountryId = 1,
                    CompanyId = 2,
                },
                new User
                {
                    UserId = 2,
                    FirstName = "Vasea",
                    LastName = "Buzu",
                    Email = "vasea.buzu@gmail.com",
                    PhoneNumber = "+37369252473",
                    Password = BCrypt.Net.BCrypt.HashPassword("z1234567"),
                    RoleId = 2,
                    CountryId = 2,
                    CompanyId = 1,
                },
                new User
                {
                    UserId = 3,
                    FirstName = "Ivan",
                    LastName = "Doe",
                    Email = "ivan.doe@gmail.com",
                    PhoneNumber = "+37364535279",
                    Password = BCrypt.Net.BCrypt.HashPassword("z1234567"),
                    RoleId = 3,
                    CountryId = 3,
                    CompanyId = 1,
                }
            );
        }
    }
}
