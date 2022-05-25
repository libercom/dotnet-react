using common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.Models
{
    public class PagedResponse
    {
        public int Count { get; set; }
        public IList<OrderDto> Orders { get; set; }
    }
}
