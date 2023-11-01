using AcmeCorp.Domain.DTO;
using AcmeCorp.Domain.Interface.Service;
using AcmeCorp.Domain.Model;
using AcmeCorp.Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcmeCorp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost("Create-Order")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto order)
        {
            var resp = await _orderService.CreateOrder(order);
            if (resp.Status == false)
            {
                return BadRequest(resp);
            }
            return Ok(resp);
        }
        [HttpGet("get-allOrder")]
        public async Task<IActionResult> GetOrder()
        {
            var resp = await _orderService.GetAllOrders();
            if (resp.Status == false)
            {
                return BadRequest(resp);
            }
            return Ok(resp);
        }

        [HttpGet("customer-total-order/{customerId}")]
        public async Task<IActionResult> CalculateCustomerOrderTotal([FromRoute]int customerId)
        {
            var resp = await _orderService.CalculateCustomerOrderTotal(customerId);
            if (resp.Status == false)
            {
                return BadRequest(resp);
            }
            return Ok(resp);
        }

        [HttpGet("orderby-id/{orderId}")]
        public async Task<IActionResult> GetOrderById([FromRoute] int orderId)
        {
            var resp = await _orderService.GetOrderById(orderId);
            if (resp.Status == false)
            {
                return BadRequest(resp);
            }
            return Ok(resp);
        }

        [HttpGet("order-bycustomerId/{customerId}")]
        public async Task<IActionResult> GetOrdersByCustomerId([FromRoute] int customerId)
        {
            var resp = await _orderService.GetOrdersByCustomerId(customerId);
            if (resp.Status == false)
            {
                return BadRequest(resp);
            }
            return Ok(resp);
        }

        [HttpDelete("delete-order/{customerId}")]
        public async Task<IActionResult> DeleteOrder([FromRoute]int customerId)
        {
            var resp = await _orderService.DeleteOrder(customerId);
            if (resp.Status == false)
            {
                return BadRequest(resp);
            }
            return Ok(resp);
        }

        [HttpPut("update-order/{orderId}")]
        public async Task<IActionResult> UpdateOrder([FromRoute] int orderId)
        {
            var resp = await _orderService.UpdateOrder(orderId);
            if (resp.Status == false)
            {
                return BadRequest(resp);
            }
            return Ok(resp);
        }

        [HttpPut("process-Customerorder/{customerId}")]
        public async Task<IActionResult> ProcessOrder([FromRoute] int customerId)
        {
            var resp = await _orderService.ProcessOrder(customerId);
            if (resp.Status == false)
            {
                return BadRequest(resp);
            }
            return Ok(resp);
        }
    }
}
