using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.Models
{
    public class PagedRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortCriteria { get; set; }
        public string SortType { get; set; }
        public int DestinationCountry { get; set; }
        public int SendingCountry { get; set; }
        public int UserId { get; set; }
    }
}
