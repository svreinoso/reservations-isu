using Reservation.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Reservation.Data.Entities
{
    public class Rate : ITablesAudit
    {
        public int Id { get; set; }
        [Required]
        public int ContactId { get; set; }
        [Required]
        public int Star { get; set; }
        public Contact Contact { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
