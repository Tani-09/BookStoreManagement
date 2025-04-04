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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService categoryservice;

        public CategoriesController(ICategoryService categoryservice) {


            this.categoryservice = categoryservice;
        }


        [Authorize(Roles = "Admin, User, Guest")]
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await categoryservice.GetAllAsync();
            return Ok(categories);
        }


        [Authorize(Roles = "Admin, User, Guest")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await categoryservice.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryRequestDTO categoryRequestDto)
        {
            var createdCategory = await categoryservice.CreateAsync(categoryRequestDto);
            return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryRequestDTO categoryRequestDto)
        {
            var updatedCategory = await categoryservice.UpdateAsync(id, categoryRequestDto);
            if (updatedCategory == null)
            {
                return NotFound();
            }
            return Ok(updatedCategory);
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await categoryservice.DeleteAsync(id);
            return NoContent();
        }
    }
}
