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
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemService orderitemservice;

        public OrderItemsController(IOrderItemService orderitemservice) {

            this.orderitemservice = orderitemservice;
        }



        [Authorize(Roles = "Admin, User, Guest")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrderItems()
        {
            var orderItems = await orderitemservice.GetAllAsync();
            return Ok(orderItems);
        }



        [Authorize(Roles = "Admin, User, Guest")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderItemById(int id)
        {
            var orderItem = await orderitemservice.GetByIdAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }
            return Ok(orderItem);
        }



        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateOrderItem([FromBody] OrderItemRequestDTO orderItemRequestDto)
        {
            var createdOrderItem = await orderitemservice.CreateAsync(orderItemRequestDto);
            return CreatedAtAction(nameof(GetOrderItemById), new { id = createdOrderItem.Id }, createdOrderItem);
        }



        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderItem(int id, [FromBody] OrderItemRequestDTO orderItemRequestDto)
        {
            var updatedOrderItem = await orderitemservice.UpdateAsync(id, orderItemRequestDto);
            if (updatedOrderItem == null)
            {
                return NotFound();
            }
            return Ok(updatedOrderItem);
        }



        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            await orderitemservice.DeleteAsync(id);
            return NoContent();
        }
    }
}
