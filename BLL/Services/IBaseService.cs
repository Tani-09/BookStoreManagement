namespace BookStoreManagement.BLL.Services
{
    public interface IBaseService<TRequestDTO, TResponseDTO>
    {
        Task<IEnumerable<TResponseDTO>> GetAllAsync();
        Task<TResponseDTO> GetByIdAsync(int id);
        Task<TResponseDTO> CreateAsync(TRequestDTO requestDTO);
        Task<TResponseDTO> UpdateAsync(int id, TRequestDTO requestDTO);
        Task DeleteAsync(int id);

    }
}
