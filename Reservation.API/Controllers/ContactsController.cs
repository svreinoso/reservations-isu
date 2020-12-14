using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservation.BL.Services;
using Reservation.Data;
using Reservation.Data.Entities;
using Reservation.Data.Models;

namespace Reservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IContactService _contactService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="contactService"></param>
        public ContactsController(ApplicationDbContext context, IContactService contactService)
        {
            _context = context;
            _contactService = contactService;
        }

        /// <summary>
        /// Get Contacts filtered and with pagination
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetContacts([FromQuery] ApiQueryOptions option)
        {
            ApiResult result = await _contactService.GetContacts(option);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// Get Contact by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
            {
                return NotFound();
            }

            return contact;
        }

        /// <summary>
        /// Get a Contact by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("GetByName/{name}")]
        public async Task<ActionResult<Contact>> GetByName(string name)
        {
            var contact = await _context.Contacts.FirstOrDefaultAsync(x => x.Name == name);
            return contact;
        }

        /// <summary>
        /// Add or edit Contact or reservations
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostContact(ContactDto contact)
        {
            await _contactService.AddContact(contact);
            return new OkResult();
        }

        /// <summary>
        /// Delete a contact
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }
    }
}
