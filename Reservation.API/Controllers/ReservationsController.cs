using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservation.BL.Services;
using Reservation.Data;
using Reservation.Data.Models;

namespace Reservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IReservationServices _reservationService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="reservationService"></param>
        public ReservationsController(ApplicationDbContext context, IReservationServices reservationService)
        {
            _context = context;
            _reservationService = reservationService;
        }

        /// <summary>
        /// Get reservation filteres, ordered and paginated
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetReservations([FromQuery] ApiQueryOptions option)
        {
            ApiResult result = await _reservationService.GetReservations(option);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// Get reservation by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservation(int id)
        {
            var reservation = await _context.Reservations
                .Include(x => x.Contact)
                .Select(x => new ContactDto {
                    ReservationId = x.Id,
                    Id = x.Contact.Id,
                    Name = x.Contact.Name,
                    ContactType = x.Contact.ContactType,
                    PhoneNumber = x.Contact.PhoneNumber,
                    BirthDate = x.Contact.BirthDate,
                    EditorData = x.EditorData,
                    UserId = x.UserId
                }).FirstOrDefaultAsync(x => x.ReservationId == id);

            if (reservation == null)
            {
                return NotFound();
            }

            return new OkObjectResult(reservation);
        }

    }
}
