using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Data.Models
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Rating { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool IsFavorite { get; set; }
    }
}
