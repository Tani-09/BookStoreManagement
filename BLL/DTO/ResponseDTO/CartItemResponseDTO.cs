namespace BookStoreManagement.BLL.DTO.ResponseDTO
{
    public class CartItemResponseDTO
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public string? BookTitle { get; set; }
    }
}
