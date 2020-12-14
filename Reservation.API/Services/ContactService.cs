//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Reservation.Data;
//using Reservation.Data.Entities;
//using Reservation.Data.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Reservation.API.Services
//{
//    public interface IContactService
//    {
//        Task<IActionResult> AddContact(ContactDto dto);
//        Task<ApiResult> GetContacts(ApiQueryOptions option);
//    }

//    public class ContactService : IContactService
//    {
//        private readonly ApplicationDbContext _context;

//        public ContactService(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<IActionResult> AddContact(ContactDto dto)
//        {
//            var contact = await _context.Contacts.FirstOrDefaultAsync(x => x.Name == dto.Name);
//            if(contact == null)
//            {
//                contact = new Contact
//                {
//                    Name = dto.Name,
//                    BirthDate = dto.BirthDate,
//                    PhoneNumber = dto.PhoneNumber,
//                    ContactType = dto.ContactType
//                };
//                _context.Contacts.Add(contact);
//                await _context.SaveChangesAsync();
//            } else
//            {
//                //if the contact is edited
//                if(contact.Name != dto.Name ||
//                    contact.BirthDate != dto.BirthDate ||
//                    contact.PhoneNumber != dto.PhoneNumber ||
//                    contact.ContactType != dto.ContactType)
//                {
//                    contact.Name = dto.Name;
//                    contact.PhoneNumber = dto.PhoneNumber;
//                    contact.PhoneNumber = dto.PhoneNumber;
//                    contact.ContactType = dto.ContactType;
//                    _context.Entry(contact).State = EntityState.Modified;
//                    await _context.SaveChangesAsync();
//                }
//            }

//            if(dto.ReservationId > 0)
//            {
//                var reservation = await _context.Reservations.FirstOrDefaultAsync(x => x.Id == dto.ReservationId);
//                if(reservation.EditorData != dto.EditorData)
//                {
//                    reservation.EditorData = dto.EditorData;
//                    _context.Entry(reservation).State = EntityState.Modified;
//                    await _context.SaveChangesAsync();
//                } 
//            }
//            else if (!string.IsNullOrEmpty(dto.EditorData))
//            {
//                // create new reservation
//                var reservation = new Data.Entities.Reservation
//                {
//                    ContactId = contact.Id,
//                    EditorData = dto.EditorData,
//                    UserId = dto.UserId
//                };
//                _context.Reservations.Add(reservation);
//                await _context.SaveChangesAsync();
//            }
//            return new OkResult();
//        }

//        public async Task<ApiResult> GetContacts(ApiQueryOptions option)
//        {
//            var query = string.IsNullOrEmpty(option.Search)
//                ? _context.Contacts.Where(x => x.Name != "")
//                : _context.Contacts.Where(x => x.Name.Contains(option.Search));

//            var count = await query.CountAsync();

//            switch (option.Sort)
//            {
//                case "0_asc":
//                    query = query.OrderBy(x => x.Name);
//                    break;
//                case "0_desc":
//                    query = query.OrderByDescending(x => x.Name);
//                    break;
//                case "1_asc":
//                    query = query.OrderBy(x => x.ContactType);
//                    break;
//                case "1_desc":
//                    query = query.OrderByDescending(x => x.ContactType);
//                    break;
//                case "2_asc":
//                    query = query.OrderBy(x => x.PhoneNumber);
//                    break;
//                case "2_desc":
//                    query = query.OrderByDescending(x => x.PhoneNumber);
//                    break;
//                case "3_asc":
//                    query = query.OrderBy(x => x.BirthDate);
//                    break;
//                case "3_desc":
//                    query = query.OrderByDescending(x => x.BirthDate);
//                    break;
//            }
//            var skip = option.Page == 1 ? 0 : (option.Page -1) * option.PageSize;
//            var data = await query.Skip(skip).Take(option.PageSize).ToListAsync();
//            return new ApiResult
//            {
//                Page = option.Page,
//                Pages = count / option.Page,
//                Total = count,
//                Data = data
//            };
//        }
//    }
//}
