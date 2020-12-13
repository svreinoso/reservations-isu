using Reservation.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Reservation.Data.Models
{
    public class ContactDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Contact name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Contact type is required")]
        public ContactType ContactType { get; set; }
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Birthdate is required")]
        public DateTime BirthDate { get; set; }
        public string EditorData { get; set; }
        public int ReservationId { get; set; }
        public string UserId { get; set; }
    }
}
