using AcmeCorp.Domain.DTO;
using AcmeCorp.Domain.Interface.Service;
using AcmeCorp.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcmeCorp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _custService;
        public CustomerController(ICustomerService custService) 
        {
            _custService = custService;
        }

        [AllowAnonymous]
        [HttpPost("Create-Customer")]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDto customer)
        {
            var resp = await _custService.CreateCustomer(customer);
            if (resp.Status == false)
            {
                return BadRequest(resp);
            }
            return Ok(resp);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO customer)
        {
            var resp = await _custService.Login(customer);
            if (resp.Status == false)
            {
                return BadRequest(resp);
            }
            return Ok(resp);
        }

        [HttpDelete("Delete-Customer")]
        public async Task<IActionResult> DeleteCustomer([FromRoute]int customerId)
        {
            var resp = await _custService.DeleteCustomer(customerId);
            if (resp.Status == false)
            {
                return BadRequest(resp);
            }
            return Ok(resp);
        }

        [HttpGet("get-allcustomer")]
        public async Task<IActionResult> GetAllCustomer()
        {
            var resp = await _custService.GetCustomers();
            if (resp.Status == false)
            {
                return BadRequest(resp);
            }
            return Ok(resp);
        }

        [HttpGet("get-customer-byid")]
        public async Task<IActionResult> GetAllCustomer([FromRoute] int customerId)
        { 
            var resp = await _custService.GetCustomer(customerId);
            if (resp.Status == false)
            {
                return BadRequest(resp);
            }
            return Ok(resp);
        }

    }
}
