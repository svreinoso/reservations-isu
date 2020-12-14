using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservation.Data;
using Reservation.Data.Entities;
using System.Threading.Tasks;

namespace Reservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public FavoriteController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Add Reservation to favorite
        /// </summary>
        /// <param name="favorite"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(FavoriteReservation favorite)
        {
            _context.FavoriteReservations.Add(favorite);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        /// <summary>
        /// Remove from favorite
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Remove")]
        public async Task<IActionResult> Remove(FavoriteReservation reservation)
        {
            var favorite = await _context.FavoriteReservations
                .FirstOrDefaultAsync(x => x.UserId == reservation.UserId && x.ReservationId == reservation.ReservationId);
            if(favorite != null)
            {
                _context.Remove(favorite);
            }
            await _context.SaveChangesAsync();
            return new OkResult();
        }
    }
}
