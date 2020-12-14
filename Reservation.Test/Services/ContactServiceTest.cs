using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Reservation.BL.Services;
using Reservation.Data;
using Reservation.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Test.Services
{
    public class ContactServiceTest
    {
        private ContactService _contactService;
        private ApplicationDbContext _db;

        [SetUp]
        public void Setup()
        {
            _db = SetTestDb.CreateContextForInMemory();
            _contactService = new ContactService(_db);
        }

        private void CleanContacts()
        {
            var contacts = _db.Contacts.ToList();
            if (contacts.Any())
            {
                _db.RemoveRange(contacts);
                _db.SaveChanges();
            }
        }

        private void CleanReservations()
        {
            var reservations = _db.Reservations.ToList();
            if (reservations.Any())
            {
                _db.Reservations.RemoveRange(reservations);
                _db.SaveChanges();
            }
        }

        /// <summary>
        /// Should add new contact without reservation
        /// </summary>
        [Test]
        public async Task AddContactTestAsync()
        {
            CleanContacts();
            CleanReservations();

            var dto = new ContactDto
            {
                Name = "name",
                ContactType = Data.Enums.ContactType.Home,
                BirthDate = DateTime.Now,
                PhoneNumber = "123"
            };

            await _contactService.AddContact(dto);

            var newContact = _db.Contacts.FirstOrDefault();
            Assert.IsNotNull(newContact);

            var reservations = _db.Reservations.ToList();

            Assert.AreEqual(0, reservations.Count);
        }


        /// <summary>
        /// Should add new contact wit reservation
        /// </summary>
        [Test]
        public async Task AddContactWithReservationTestAsync()
        {
            CleanContacts();
            CleanReservations();

            var dto = new ContactDto
            {
                Name = "name",
                ContactType = Data.Enums.ContactType.Home,
                BirthDate = DateTime.Now,
                PhoneNumber = "123",
                EditorData = "<p>test data</p>"
            };

            await _contactService.AddContact(dto);

            var newContact = _db.Contacts.FirstOrDefault();
            Assert.IsNotNull(newContact);

            var reservations = _db.Reservations.ToList();

            Assert.AreEqual(1, reservations.Count);
        }


        /// <summary>
        /// Should edit reservation
        /// </summary>
        [Test]
        public async Task EditReservationTestAsync()
        {
            CleanContacts();
            CleanReservations();

            var dto = new ContactDto
            {
                Name = "name",
                ContactType = Data.Enums.ContactType.Home,
                BirthDate = DateTime.Now,
                PhoneNumber = "123",
                EditorData = "<p>test data</p>"
            };

            await _contactService.AddContact(dto);

            var newContact = _db.Contacts.FirstOrDefault();
            Assert.IsNotNull(newContact);

            var reservations = _db.Reservations.ToList();

            Assert.AreEqual(1, reservations.Count);

            var reservation = _db.Reservations.FirstOrDefault();
            dto.Id = newContact.Id;
            dto.ReservationId = reservation.Id;
            dto.EditorData = "edited";
            await _contactService.AddContact(dto);

            reservations = _db.Reservations.ToList();

            Assert.AreEqual(1, reservations.Count);

            reservation = _db.Reservations.FirstOrDefault();
            Assert.AreEqual(dto.EditorData, reservation.EditorData);
        }

        /// <summary>
        /// Should add new reservation for existing contact
        /// </summary>
        [Test]
        public async Task AddNewReservationForExistingContactTestAsync()
        {
            CleanContacts();
            CleanReservations();

            var dto = new ContactDto
            {
                Name = "name",
                ContactType = Data.Enums.ContactType.Home,
                BirthDate = DateTime.Now,
                PhoneNumber = "123",
                EditorData = "<p>test data</p>"
            };

            await _contactService.AddContact(dto);

            var newContact = _db.Contacts.FirstOrDefault();
            Assert.IsNotNull(newContact);

            var reservations = _db.Reservations.ToList();

            Assert.AreEqual(1, reservations.Count);

            var reservation = new ContactDto
            {
                Name = "name",
                ContactType = Data.Enums.ContactType.Home,
                BirthDate = DateTime.Now,
                PhoneNumber = "123",
                EditorData = "<p>test data 2</p>"
            };

            await _contactService.AddContact(reservation);

            reservations = _db.Reservations.ToList();

            var contactsCount = _db.Contacts.Count();
            Assert.AreEqual(1, contactsCount);
            Assert.AreEqual(2, reservations.Count);
        }
    }
}
