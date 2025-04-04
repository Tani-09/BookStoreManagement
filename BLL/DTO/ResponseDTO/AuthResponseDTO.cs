namespace BookStoreManagement.BLL.DTO.ResponseDTO
{
    public class AuthResponseDTO
    {
        public string? Token { get; set; }

        public string? Role { get; set; }
        public string? Error { get; set; }
    }
}
