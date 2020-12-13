using Microsoft.EntityFrameworkCore;
using Reservation.Data;
using Reservation.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation.BL.Services
{
    public interface IReservationServices
    {
        Task<ApiResult> GetReservations(ApiQueryOptions option);
    }

    public class ReservationServices : IReservationServices
    {
        private readonly ApplicationDbContext _context;

        public ReservationServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResult> GetReservations(ApiQueryOptions option)
        {
            var query  = _context.Reservations.Select(x => new
            {
                x.Id,
                x.Contact.Name,
                x.Rating,
                x.CreatedDate,
                IsFavorite = _context.FavoriteReservations.Any(r => r.UserId == option.CurrentUser && r.ReservationId == x.Id)
            });

            var count = await query.CountAsync();

            switch(option.Sort)
            {
                case "1":
                    query = query.OrderBy(x => x.CreatedDate);
                    break;
                case "2":
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
                case "3":
                    query = query.OrderBy(x => x.Name);
                    break;
                case "4":
                    query = query.OrderByDescending(x => x.Name);
                    break;
                case "5":
                    query = query.OrderBy(x => x.Rating);
                    break;
                case "6":
                    query = query.OrderByDescending(x => x.Rating);
                    break;
            }

            var skip = option.Page == 1 ? 0 : (option.Page - 1) * option.PageSize;
            var data = await query.Skip(skip).Take(option.PageSize).ToListAsync();

            return new ApiResult
            {
                Page = option.Page,
                Pages = count / option.Page,
                Total = count,
                Data = data
            };
        }
    }
}
