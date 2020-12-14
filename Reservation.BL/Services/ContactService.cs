﻿using Microsoft.EntityFrameworkCore;
using Reservation.Data;
using Reservation.Data.Entities;
using Reservation.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation.BL.Services
{
    public interface IContactService
    {
        Task<ApiResult> AddContact(ContactDto dto);
        Task<ApiResult> GetContacts(ApiQueryOptions option);
    }

    public class ContactService : IContactService
    {
        private readonly ApplicationDbContext _context;

        public ContactService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Add or Edit Contact or Reservation
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ApiResult> AddContact(ContactDto dto)
        {
            var contact = await _context.Contacts.FirstOrDefaultAsync(x => x.Name == dto.Name);
            //If the name does not exist, create the contact
            if(contact == null)
            {
                contact = new Contact
                {
                    Name = dto.Name,
                    BirthDate = dto.BirthDate,
                    PhoneNumber = dto.PhoneNumber,
                    ContactType = dto.ContactType
                };
                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();
            } else
            {
                //if the contact exist, check if there are changes to edit
                if(contact.Name != dto.Name ||
                    contact.BirthDate != dto.BirthDate ||
                    contact.PhoneNumber != dto.PhoneNumber ||
                    contact.ContactType != dto.ContactType)
                {
                    contact.Name = dto.Name;
                    contact.PhoneNumber = dto.PhoneNumber;
                    contact.PhoneNumber = dto.PhoneNumber;
                    contact.ContactType = dto.ContactType;
                    _context.Entry(contact).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
            }

            //If the reservation exist check if has changes and update it
            if(dto.ReservationId > 0)
            {
                var reservation = await _context.Reservations.FirstOrDefaultAsync(x => x.Id == dto.ReservationId);
                if(reservation.EditorData != dto.EditorData)
                {
                    reservation.EditorData = dto.EditorData;
                    _context.Entry(reservation).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                } 
            }
            else if (!string.IsNullOrEmpty(dto.EditorData))
            {
                // create new reservation
                var reservation = new Data.Entities.Reservation
                {
                    ContactId = contact.Id,
                    EditorData = dto.EditorData,
                    UserId = dto.UserId
                };
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();
            }
            return new ApiResult();
        }

        /// <summary>
        /// Get Contacts filteres, ordered and paginated
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public async Task<ApiResult> GetContacts(ApiQueryOptions option)
        {
            var query = string.IsNullOrEmpty(option.Search)
                ? _context.Contacts.Where(x => x.Name != "")
                : _context.Contacts.Where(x => x.Name.Contains(option.Search));

            var count = await query.CountAsync();

            switch (option.Sort)
            {
                case "0_asc":
                    query = query.OrderBy(x => x.Name);
                    break;
                case "0_desc":
                    query = query.OrderByDescending(x => x.Name);
                    break;
                case "1_asc":
                    query = query.OrderBy(x => x.ContactType);
                    break;
                case "1_desc":
                    query = query.OrderByDescending(x => x.ContactType);
                    break;
                case "2_asc":
                    query = query.OrderBy(x => x.PhoneNumber);
                    break;
                case "2_desc":
                    query = query.OrderByDescending(x => x.PhoneNumber);
                    break;
                case "3_asc":
                    query = query.OrderBy(x => x.BirthDate);
                    break;
                case "3_desc":
                    query = query.OrderByDescending(x => x.BirthDate);
                    break;
            }
            var skip = option.Page == 1 ? 0 : (option.Page -1) * option.PageSize;
            var data = await query.Skip(skip).Take(option.PageSize).ToListAsync();
            return new ApiResult
            {
                Page = option.Page,
                Pages = (int)Math.Ceiling(count / (decimal)option.PageSize),
                Total = count,
                Data = data
            };
        }
    }
}
