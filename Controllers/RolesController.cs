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
    public class RolesController : ControllerBase
    {
        private readonly IRoleService roleservice;

        public RolesController(IRoleService roleservice) {

            this.roleservice = roleservice;
        }


        [Authorize(Roles = "Admin, User, Guest")]
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await roleservice.GetAllAsync();
            return Ok(roles);
        }


        [Authorize(Roles = "Admin, User, Guest")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await roleservice.GetByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleRequestDTO roleRequestDto)
        {
            var createdRole = await roleservice.CreateAsync(roleRequestDto);
            return CreatedAtAction(nameof(GetRoleById), new { id = createdRole.Id }, createdRole);
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] RoleRequestDTO roleRequestDto)
        {
            var updatedRole = await roleservice.UpdateAsync(id, roleRequestDto);
            if (updatedRole == null)
            {
                return NotFound();
            }
            return Ok(updatedRole);
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            await roleservice.DeleteAsync(id);
            return NoContent();
        }
    }
}
