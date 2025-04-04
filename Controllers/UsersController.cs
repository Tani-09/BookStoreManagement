using BookStoreManagement.BLL.DTO.RequestDTO;
using BookStoreManagement.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userservice;

        public UsersController(IUserService userservice) {


            this.userservice = userservice;
        }


        [Authorize(Roles = "Admin, User, Guest")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await userservice.GetAllAsync();
            return Ok(users);
        }


        [Authorize(Roles = "Admin, User, Guest")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await userservice.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserRequestDTO userRequestDto)
        {
            var createdUser = await userservice.CreateAsync(userRequestDto);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserRequestDTO userRequestDto)
        {
            var updatedUser = await userservice.UpdateAsync(id, userRequestDto);
            if (updatedUser == null)
            {
                return NotFound();
            }
            return Ok(updatedUser);
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await userservice.DeleteAsync(id);
            return NoContent();
        }
    }
}
