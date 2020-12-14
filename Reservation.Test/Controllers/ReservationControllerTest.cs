using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Reservation.API.Controllers;
using Reservation.BL.Services;
using Reservation.Data;
using Reservation.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reservation.Test.Controllers
{
    public class ReservationControllerTest
    {
        private ApplicationDbContext _context;
        private IReservationServices _reservationServices;
        private ReservationsController _reservationsController;
        private ContactService _contactService;

        [SetUp]
        public void Setup()
        {
            _context = SetTestDb.CreateContextForInMemory();
            _reservationServices = new ReservationServices(_context);
            _contactService = new ContactService(_context);
            _reservationsController = new ReservationsController(_context, _reservationServices);
        }

        private async Task AddReservationsAsync()
        {
            var dto = new ContactDto
            {
                Name = "name",
                ContactType = Data.Enums.ContactType.Home,
                BirthDate = DateTime.Now,
                PhoneNumber = "123",
                EditorData = "data"
            };

            await _contactService.AddContact(dto);
            dto.EditorData = "data 2";
            await _contactService.AddContact(dto);
            dto.EditorData = "data 3";
            await _contactService.AddContact(dto);
        }

        /// <summary>
        /// Should return reservation list
        /// </summary>
        [Test]
        public async Task GetReservationsTestAsync()
        {
            await AddReservationsAsync();

            var options = new ApiQueryOptions
            {
                Page = 1,
                PageSize = 2,
            };
            var actionResuolt = await _reservationsController.GetReservations(options);
            var okResult = actionResuolt as OkObjectResult;
            var apiResult = okResult.Value as ApiResult;
            Assert.AreEqual(apiResult.Page, 1);
            Assert.AreEqual(apiResult.Pages, 2);
            Assert.AreEqual(apiResult.Total, 3);
            var data = apiResult.Data as List<ReservationDto>;
            Assert.AreEqual(data.Count, 2);

            options.PageSize = 10;

            actionResuolt = await _reservationsController.GetReservations(options);
            okResult = actionResuolt as OkObjectResult;
            apiResult = okResult.Value as ApiResult;
            Assert.AreEqual(apiResult.Pages, 1);
            data = apiResult.Data as List<ReservationDto>;
            Assert.AreEqual(data.Count, 3);
        }

        /// <summary>
        /// Should return a empty reservation list
        /// </summary>
        [Test]
        public async Task GetEmptyReservationsTestAsync()
        {
            var options = new ApiQueryOptions
            {
                Page = 1,
                PageSize = 10,
            };
            var actionResuolt = await _reservationsController.GetReservations(options);
            var okResult = actionResuolt as OkObjectResult;
            var apiResult = okResult.Value as ApiResult;
            Assert.AreEqual(apiResult.Page, 1);
            Assert.AreEqual(apiResult.Pages, 0);
            Assert.AreEqual(apiResult.Total, 0);
            var data = apiResult.Data as List<ReservationDto>;
            Assert.AreEqual(data.Count, 0);
        }
    }
}
