using AutoMapper;
using BookStoreManagement.BLL.DTO.RequestDTO;
using BookStoreManagement.BLL.DTO.ResponseDTO;
using BookStoreManagement.DAL.Entities;
using BookStoreManagement.DAL.Repositories;

namespace BookStoreManagement.BLL.Services
{
    public interface IOrderService: IBaseService <OrderRequestDTO, OrderResponseDTO>
    {
    }

    public class OrderService : IOrderService
    {
        private readonly IRepositoryWrapper repositorywrapper;
        private readonly IMapper mapper;

        public OrderService(IRepositoryWrapper repositorywrapper, IMapper mapper)
            {
            this.repositorywrapper = repositorywrapper;
            this.mapper = mapper;

        }

        public async Task<IEnumerable<OrderResponseDTO>> GetAllAsync()
        {
            var orders = await repositorywrapper.OrderRepository.GetAllAsync();
            return mapper.Map<IEnumerable<OrderResponseDTO>>(orders);

        }

        public async Task<OrderResponseDTO> GetByIdAsync(int id)
        {
            var order = await repositorywrapper.OrderRepository.GetOrderAsync(id);
            return mapper.Map<OrderResponseDTO>(order);
        }

        public async Task<OrderResponseDTO> CreateAsync(OrderRequestDTO requestdto)
        {
           // var order = mapper.Map<Order>(requestdto);



            var createdorder = await repositorywrapper.OrderRepository.CreateOrderAsync(requestdto);
            return mapper.Map<OrderResponseDTO>(createdorder);
        }

        public async Task<OrderResponseDTO> UpdateAsync(int id, OrderRequestDTO requestdto)
        {
            var order = mapper.Map<Order>(requestdto);

            var updatedorder = await repositorywrapper.OrderRepository.UpdateAsync(id, order);
            return mapper.Map<OrderResponseDTO>(updatedorder);
        }

        public async Task DeleteAsync(int id)
        {
            var order = await repositorywrapper.OrderRepository.GetByIdAsync(id);
            if (order != null)
            {
                await repositorywrapper.OrderRepository.DeleteAsync(order);
            }
        }

    }
}
