using Reservation.Data.Interfaces;
using System;

namespace Reservation.Data.Entities
{
    public class Reservation : ITablesAudit
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public Contact Contact { get; set; }
        public string EditorData { get; set; }
        public decimal Rating { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
