using ContactAPI.Data;
using ContactAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ContactAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly ContactAPIDbContext dbContext;
        public ContactsController(ContactAPIDbContext dbContext)
        {
            this.dbContext = dbContext; 
        }
        [HttpGet] 
        public async Task<IActionResult> GetContacts()
        {
            return Ok((await dbContext.Contacts.ToListAsync()));
        }

        [HttpGet]
        [Route ("{id:guid}")]
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);
            if (contact != null) { 
                
                return Ok(contact);
            
            }
            return NotFound();

        }

        [HttpPost]
        public async Task<IActionResult> AddContact(AddContactRequest addcontact)
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                FullName = addcontact.FullName,
                Email = addcontact.Email,
                Phone = addcontact.Phone,
                Address = addcontact.Address
            };

            await dbContext.Contacts.AddAsync(contact);
            await dbContext.SaveChangesAsync();

            return Ok(contact); 

        }

        [HttpPut]
        [Route ("{id:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContactRequest updateContact)
        {
            var contact = await dbContext.Contacts.FindAsync(id);
            if (contact != null) {

                contact.FullName = updateContact.FullName;
                contact.Email = updateContact.Email;
                contact.Phone = updateContact.Phone;
                contact.Address = updateContact.Address;

                await dbContext.SaveChangesAsync();
                
                return Ok(contact);

            }

            return NotFound();
        }

        [HttpDelete]
        [Route ("{id:guid}")]

        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var contact = await  dbContext.Contacts.FindAsync(id);

            if (contact != null)
            {
                dbContext.Contacts.Remove(contact);
                await dbContext.SaveChangesAsync();

                return Ok(contact);
            }
            return NotFound();
        }

    }
}
