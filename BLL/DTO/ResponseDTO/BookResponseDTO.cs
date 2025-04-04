namespace BookStoreManagement.BLL.DTO.ResponseDTO
{
    public class BookResponseDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public double Price { get; set; }
        public string? CategoryName { get; set; }
        public string? ISBN { get; set; }
        public int StockQuantity { get; set; }
    }
}
