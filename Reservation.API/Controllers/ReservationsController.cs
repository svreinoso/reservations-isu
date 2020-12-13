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

        public ReservationsController(ApplicationDbContext context, IReservationServices reservationService)
        {
            _context = context;
            _reservationService = reservationService;
        }

        // GET: api/Reservations
        [HttpGet]
        public async Task<ActionResult> GetReservations([FromQuery] ApiQueryOptions option)
        {
            ApiResult result = await _reservationService.GetReservations(option);
            return new OkObjectResult(result);
        }

        // GET: api/Reservations/5
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

        // PUT: api/Reservations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservation(int id, Data.Entities.Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return BadRequest();
            }

            _context.Entry(reservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Reservations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Data.Entities.Reservation>> PostReservation(Data.Entities.Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReservation", new { id = reservation.Id }, reservation);
        }

        // DELETE: api/Reservations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
