using Reservation.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Data.Entities
{
    public class UserFavoriteReservation : ITablesAudit
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public string UserId { get; set; }
        public Contact Contact { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
