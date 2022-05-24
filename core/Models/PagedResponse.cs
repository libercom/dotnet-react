using core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Models
{
    public class PagedResponse
    {
        public int Count { get; set; }
        public IList<OrderDto> Orders { get; set; }
    }
}
