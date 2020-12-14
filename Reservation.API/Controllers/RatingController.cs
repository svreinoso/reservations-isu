using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservation.Data;
using Reservation.Data.Entities;
using System.Threading.Tasks;

namespace Reservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public RatingController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Add new rating and update the value for the reservation
        /// </summary>
        /// <param name="rating"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostContact(Rating rating)
        {
            var sql = $@"EXEC SP_UpdateRating {rating.Star}, {rating.ReservationId}, '{rating.UserId}'";
            await _context.Database.ExecuteSqlRawAsync(sql);
            return new OkResult();
        }
    }
}
