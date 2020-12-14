using Reservation.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Data.Entities
{
    public class User : ITablesAudit
    {
        public int Id { get; set; }
        public string BrowserId { get; set; }

        public List<FavoriteReservation> FavoriteReservations { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
