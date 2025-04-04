using BookStoreManagement.BLL.DTO;
using BookStoreManagement.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookStoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }




        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLogin)
        {

            if (userLogin == null || string.IsNullOrEmpty(userLogin.Username) || string.IsNullOrEmpty(userLogin.Password))
            {
                return BadRequest("Invalid client request");
            }


            var response =  await _authService.Login(userLogin);


            if (!string.IsNullOrEmpty(response.Error))
            {
                return Unauthorized(response.Error);
            }


            return Ok(new { token = response.Token, role = response.Role });
        }
           
    }
}

