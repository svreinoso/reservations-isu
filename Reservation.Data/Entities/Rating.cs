using Reservation.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Reservation.Data.Entities
{
    public class Rating : ITablesAudit
    {
        public int Id { get; set; }
        [Required]
        public int ReservationId { get; set; }
        [Required]
        public int Star { get; set; }
        public string UserId { get; set; }
        public Reservation Reservation { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
