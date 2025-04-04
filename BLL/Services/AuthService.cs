using BookStoreManagement.BLL.DTO;
using BookStoreManagement.BLL.DTO.ResponseDTO;
using BookStoreManagement.DAL.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStoreManagement.BLL.Services
{
    public class AuthService
    {

        private readonly JwtSettings _jwtSettings;
        private readonly IUserService _userService;

        public AuthService(IOptions<JwtSettings> jwtSettings, IUserService userService)
        {
            _jwtSettings = jwtSettings.Value ?? throw new ArgumentNullException(nameof(jwtSettings));
            _userService = userService;
        }


        public string GenerateToken(User user)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
             new Claim(ClaimTypes.Role, user.Role?.Name )
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //ROLE BASED AUTHENTICATION
        public async Task<AuthResponseDTO> Login(UserLoginDTO userLogin)
        {

            var response = new AuthResponseDTO();

            if (userLogin == null || string.IsNullOrEmpty(userLogin.Username) || string.IsNullOrEmpty(userLogin.Password))
            {
                response.Error = "Invalid user data";
                return response;
            }

            var user = await _userService.GetUserByUsernameAsync(userLogin.Username);       //userService.GetUserByUsernameAsync(userLogin.Username);
            if (user == null || !VerifyPassword(userLogin.Password, user.Password)) // Implement password verification
            {
                response.Error = "Unauthorized";
                return response;
            }

            var token = GenerateToken(user); // Pass the user object to generate the token
            response.Token = GenerateToken(user); // Call the method directly
            response.Role = user.Role?.Name;

            return response;
        }


        private bool VerifyPassword(string password, string passwordHash)
        {
            // Use BCrypt to verify the password
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
