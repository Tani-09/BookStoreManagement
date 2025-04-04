using BookStoreManagement.DAL.Entities;

namespace BookStoreManagement.BLL.DTO.RequestDTO
{
    public class OrderItemRequestDTO
    {
        
        public int BookId { get; set; }
        public int Quantity { get; set; }
       
    }
}
