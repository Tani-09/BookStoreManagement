namespace BookStoreManagement.BLL.DTO.RequestDTO
{
    public class BookRequestDTO
    {
        public string? Title{ get; set; }
        public string? Author { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public string? ISBN { get; set; }
        public int StockQuantity { get; set; }
    }
}
