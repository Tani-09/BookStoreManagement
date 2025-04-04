using BookStoreManagement.BLL.DTO.RequestDTO;
using BookStoreManagement.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderservice;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderservice, ILogger<OrdersController> logger) {

            this.orderservice = orderservice;
            _logger = logger;
        }


        [Authorize(Roles = "Admin, User, Guest")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await orderservice.GetAllAsync();
            return Ok(orders);
        }



        [Authorize(Roles = "Admin, User, Guest")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var order = await orderservice.GetByIdAsync(id);
                if (order == null)
                {
                    return NotFound(new { message = "Order Not Found" });
                }
                return Ok(order);
            }
            catch(Exception ex) 
            
            {
                _logger.LogError($"Error fetching order {id}: {ex.Message}");
                return StatusCode(500, new { Message = "An error occurred while retrieving the order." });
            }
           
        }



        [Authorize(Roles = "Admin, User, Guest")]
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequestDTO orderRequestDto)
        {
            var createdOrder = await orderservice.CreateAsync(orderRequestDto);
            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
        }



        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderRequestDTO orderRequestDto)
        {
            var updatedOrder = await orderservice.UpdateAsync(id, orderRequestDto);
            if (updatedOrder == null)
            {
                return NotFound();
            }
            return Ok(updatedOrder);
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await orderservice.DeleteAsync(id);
            return NoContent();
        }
    }
}
