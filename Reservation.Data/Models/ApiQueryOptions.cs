using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Data.Models
{
    public class ApiQueryOptions
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Sort { get; set; }
        public string CurrentUser { get; set; }
        public string Search { get; set; }
    }
}
