using BookStoreManagement.DAL.Entities;

namespace BookStoreManagement.BLL.DTO.ResponseDTO
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
       

        public string? Email { get; set; }
        public string? Password { get; set; }


        public string? Username { get; set; }

        public int RoleId { get; set; }  // Added RoleId as foreign key for the relationship
        public Role? Role { get; set; }
    }
}
