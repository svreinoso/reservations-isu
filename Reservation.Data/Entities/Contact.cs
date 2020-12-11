using Reservation.Data.Enums;
using Reservation.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Data.Entities
{
    public class Contact : ITablesAudit
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Contact name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Contact type is required")]
        public ContactType ContactType { get; set; }
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Birthdate is required")]
        public DateTime BirthDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
