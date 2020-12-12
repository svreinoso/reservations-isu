using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservation.API.Services;
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

        public FavoriteController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostContact(UserFavoriteReservation reservation)
        {
            _context.UserFavoriteReservations.Add(reservation);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        [HttpPost]
        [Route("Remove")]
        public async Task<IActionResult> Remove(UserFavoriteReservation reservation)
        {
            var favorite = await _context.UserFavoriteReservations
                .FirstOrDefaultAsync(x => x.UserId == reservation.UserId && x.ContactId == reservation.ContactId);
            if(favorite != null)
            {
                _context.Remove(favorite);
            }
            await _context.SaveChangesAsync();
            return new OkResult();
        }
    }
}
