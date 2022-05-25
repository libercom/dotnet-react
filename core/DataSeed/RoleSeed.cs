using domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.DataSeed
{
    public static class RoleSeed
    {
        public static void SeedRole(this ModelBuilder builder)
        {
            builder.Entity<Role>().HasData(
                new Role
                {
                    RoleId = 1,
                    RoleType = "User"
                },
                new Role
                {
                    RoleId = 2,
                    RoleType = "Editor"
                },
                new Role
                {
                    RoleId = 3,
                    RoleType = "Admin"
                }
            );
        }
    }
}
