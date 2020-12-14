using Reservation.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Data.Entities
{
    public class FavoriteReservation : ITablesAudit
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public string UserId { get; set; }
        public Reservation Reservation { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
