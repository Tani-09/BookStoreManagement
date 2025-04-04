using Azure;
using BookStoreManagement.BLL.DTO.RequestDTO;
using BookStoreManagement.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.PortableExecutable;
using System;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class BooksController : ControllerBase
    {
        private readonly IBookService bookservice;

        public BooksController(IBookService bookservice) {

            this.bookservice = bookservice;
        }




        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await bookservice.GetAllAsync();
            return Ok(books);
        }



        [Authorize(Roles = "Admin,User ,Guest")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooksById(int id)
        {
            var book = await bookservice.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }



        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] BookRequestDTO bookRequestDto)
        {
            var createdBook = await bookservice.CreateAsync(bookRequestDto);

           // CreatedAtAction simplifies the process of returning a 201 response with a Location header and response body.
           // By using nameof(GetBookById) and new { id = createdBook.Id }, you dynamically generate the URL where the client can find the newly created resource.
           // The client gets both the resource and the URL to access it.


           //return OK(createdBook) would have worked but is not following REST protocol
            return CreatedAtAction(nameof(GetBooksById), new { id = createdBook.Id }, createdBook);
        }





        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookRequestDTO bookRequestDto)
        {
            var updatedBook = await bookservice.UpdateAsync(id, bookRequestDto);
            if (updatedBook == null)
            {
                return NotFound();
            }
            return Ok(updatedBook);
        }





        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            await bookservice.DeleteAsync(id);
            return NoContent();
        }
    }
}
