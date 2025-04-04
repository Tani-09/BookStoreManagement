using AutoMapper;
using BookStoreManagement.BLL.DTO.RequestDTO;
using BookStoreManagement.BLL.DTO.ResponseDTO;
using BookStoreManagement.DAL.Entities;
using BookStoreManagement.DAL.Repositories;

namespace BookStoreManagement.BLL.Services
{
    public interface IOrderItemService: IBaseService<OrderItemRequestDTO, OrderItemResponseDTO>
    {

    }

    public class OrderItemService: IOrderItemService
    {
        private readonly IRepositoryWrapper repositorywrapper;
        private readonly IMapper mapper;

        public OrderItemService(IRepositoryWrapper repositorywrapper, IMapper mapper)
        {

            this.repositorywrapper = repositorywrapper;
            this.mapper = mapper;
        }


        public async Task<IEnumerable<OrderItemResponseDTO>> GetAllAsync()
        {
            var orderitems = await repositorywrapper.OrderItemRepository.GetAllAsync();
            return mapper.Map<IEnumerable<OrderItemResponseDTO>>(orderitems);

        }

        public async Task<OrderItemResponseDTO> GetByIdAsync(int id)
        {
            var orderitem = await repositorywrapper.OrderItemRepository.GetByIdAsync(id);
            return mapper.Map<OrderItemResponseDTO>(orderitem);
        }

        public async Task<OrderItemResponseDTO> CreateAsync(OrderItemRequestDTO requestdto)
        {
            var orderitem = mapper.Map<OrderItem>(requestdto);
            var createdorderitem = await repositorywrapper.OrderItemRepository.CreateAsync(orderitem);
            return mapper.Map<OrderItemResponseDTO>(createdorderitem);
        }

        public async Task<OrderItemResponseDTO> UpdateAsync(int id, OrderItemRequestDTO requestdto)
        {
            var orderitem = mapper.Map<OrderItem>(requestdto);
            var updatedorderitem = await repositorywrapper.OrderItemRepository.UpdateAsync(id, orderitem);
            return mapper.Map<OrderItemResponseDTO>(updatedorderitem);
        }

        public async Task DeleteAsync(int id)
        {
            var orderitem = await repositorywrapper.OrderItemRepository.GetByIdAsync(id);
            if (orderitem != null)
            {
                await repositorywrapper.OrderItemRepository.DeleteAsync(orderitem);
            }
        }

       
    }
}
