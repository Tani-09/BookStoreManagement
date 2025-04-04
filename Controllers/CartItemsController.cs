using BookStoreManagement.BLL.DTO.RequestDTO;
using BookStoreManagement.BLL.Services;
using BookStoreManagement.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookStoreManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly ICartItemService cartitemservice;

        public CartItemsController(ICartItemService cartitemservice) {

            this.cartitemservice = cartitemservice;
        }



        [Authorize(Roles = "Admin,User ,Guest")]
        [HttpGet]
        public async Task<IActionResult> GetAllCartItems()
        {
            var cartitems = await cartitemservice.GetAllAsync();
            return Ok(cartitems);
        }



        [Authorize(Roles = "Admin,User ,Guest")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCartItemById(int id)
        {
            var cartitem = await cartitemservice.GetByIdAsync(id);
            if (cartitem == null)
            {
                return NotFound();
            }
            return Ok(cartitem);
        }




        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCartItem([FromBody] CartItemRequestDTO cartitemRequestDto)
        {
            var createdCartitem = await cartitemservice.CreateAsync(cartitemRequestDto);
            return CreatedAtAction(nameof(GetCartItemById), new { id = createdCartitem.Id }, createdCartitem);
        }



        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCartItem(int id, [FromBody] CartItemRequestDTO cartitemRequestDto)
        {
            var updatedCartItem = await cartitemservice.UpdateAsync(id, cartitemRequestDto);
            if (updatedCartItem == null)
            {
                return NotFound();
            }
            return Ok(updatedCartItem);
        }



        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartItem(int id)
        {
            await cartitemservice.DeleteAsync(id);
            return NoContent();
        }



        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier); // Adjust based on your claims setup
        }



        //for user to post or add in cart

        [Authorize(Roles = "Admin, User")]
        [HttpPost("user")]
        public IActionResult CreateUserCartItem([FromBody] CartItemRequestDTO cartItemdto)
        {
            // Retrieve the current user's ID from claims or context
            var userIdStr = GetCurrentUserId(); // Implement this method to get the current user's ID

            if (!int.TryParse(userIdStr, out int userId))
            {
                return BadRequest("Invalid user ID.");
            }

            // Set the UserId in the DTO if your service method requires it
            cartItemdto.UserId = userId; // Ensure CartItemRequestDto has a UserId property

            // Call the service to create the cart item
            var createdCartItem = cartitemservice.CreateAsync(cartItemdto);

            // Return a response indicating the item was created
            return CreatedAtAction(nameof(GetCartItemById), new { id = createdCartItem.Id }, createdCartItem);
        }
    }
}
