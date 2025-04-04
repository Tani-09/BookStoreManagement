using BookStoreManagement.DAL.Entities;

namespace BookStoreManagement.BLL.DTO.RequestDTO
{
    public class CartItemRequestDTO
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }

        public Book? Book { get; set; }
    }
}
