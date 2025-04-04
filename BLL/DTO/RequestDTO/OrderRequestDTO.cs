using BookStoreManagement.DAL.Entities;

namespace BookStoreManagement.BLL.DTO.RequestDTO
{
    public class OrderRequestDTO
    {
       
        public int UserId { get; set; }
       
        public List<OrderItemRequestDTO>? OrderItems { get; set; }  // List of Order Items
        
    }
}
