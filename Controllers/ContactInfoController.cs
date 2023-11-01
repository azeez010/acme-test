using AcmeCorp.Domain.DTO;
using AcmeCorp.Domain.Interface.Service;
using AcmeCorp.Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcmeCorp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactInfoController : ControllerBase
    {
        private readonly IContactInfoService  _contactService;
        public ContactInfoController(IContactInfoService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet("get-customer/{contactId}")]
        public async Task<IActionResult> CreateCustomer([FromRoute] int contactId)
        {
            var resp = await _contactService.GetContact(contactId);
            if (resp.Status == false)
            {
                return BadRequest(resp);
            }
            return Ok(resp);
        }

        [HttpPost("create-contactInfo")]
        public async Task<IActionResult> AddContactInfo([FromBody] AddContactInfoDto contact)
        {
            var resp = await _contactService.AddContactInfo(contact);
            if (resp.Status == false)
            {
                return BadRequest(resp);
            }
            return Ok(resp);
        }

        [HttpPost("create-contactInfo/{contactId}")]
        public async Task<IActionResult> UpdateConactInfo([FromRoute]int contactId, [FromBody] UpdateContactInforDto contact)
        {
            var resp = await _contactService.UpdateConactInfo(contactId, contact);
            if (resp.Status == false)
            {
                return BadRequest(resp);
            }
            return Ok(resp);
        }

        [HttpDelete("delete-contactInfo/{contactId}")]
        public async Task<IActionResult> DeleteContactInfo([FromRoute] int contactId)
        {
            var resp = await _contactService.DeleteContactInfo(contactId);
            if (resp.Status == false)
            {
                return BadRequest(resp);
            }
            return Ok(resp);
        }
    }
}
