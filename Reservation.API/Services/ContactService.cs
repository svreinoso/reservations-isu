using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservation.Data;
using Reservation.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation.API.Services
{
    public interface IContactService
    {
        Task<ApiResult> GetReservations(ApiQueryOptions option);
    }

    public class ContactService : IContactService
    {
        private readonly ApplicationDbContext _context;

        public ContactService(ApplicationDbContext context)
        {
            _context = context;
        }

        async Task<ApiResult> IContactService.GetReservations(ApiQueryOptions option)
        {
            var contacts = await _context.Contacts.Select(x => new
            {
                x.Id,
                x.Name,
                x.PhoneNumber,
                x.Rating,
                x.CreatedDate,
                x.BirthDate,
                x.UserId,
                IsFavorite = _context.UserFavoriteReservations.Any(r => r.UserId == option.CurrentUser && r.ContactId == x.Id)
            }).ToListAsync();
            return new ApiResult
            {
                Page = option.Page,
                Pages = 1,
                Data = contacts
            };
        }
    }
}
