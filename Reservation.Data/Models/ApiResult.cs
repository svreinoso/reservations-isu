using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Data.Models
{
    public class ApiResult
    {
        public int Page { get; set; }
        public int Pages { get; set; }
        public object Data { get; set; }
    }
}
