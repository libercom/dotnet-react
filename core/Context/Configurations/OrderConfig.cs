using domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Context.Configurations
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasOne(o => o.SendingCountry)
                .WithMany()
                .HasForeignKey(o => o.SendingCountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.DestinationCountry)
                .WithMany()
                .HasForeignKey(o => o.DestinationCountryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
