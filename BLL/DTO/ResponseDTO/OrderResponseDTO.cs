using BookStoreManagement.DAL.Entities;

namespace BookStoreManagement.BLL.DTO.ResponseDTO
{
    public class OrderResponseDTO
    {
        public int Id { get; set; }
        public string? OrderName { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }

        
        public User? User { get; set; }
       
        public decimal TotalAmount { get; set; }
    }
}
