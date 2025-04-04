using AutoMapper;
using BookStoreManagement.BLL.DTO.RequestDTO;
using BookStoreManagement.BLL.DTO.ResponseDTO;
using BookStoreManagement.DAL.DBContext;
using BookStoreManagement.DAL.Entities;
using BookStoreManagement.DAL.Repositories;

namespace BookStoreManagement.BLL.Services
{
    public interface ICartItemService: IBaseService<CartItemRequestDTO, CartItemResponseDTO>
    {

    }

    public class CartItemService: ICartItemService {

        private readonly IRepositoryWrapper repositorywrapper;
        private readonly IMapper mapper;

        public CartItemService(IRepositoryWrapper repositorywrapper, IMapper mapper)
        {
            this.repositorywrapper = repositorywrapper;
            this.mapper = mapper;
        }


        public async Task<IEnumerable<CartItemResponseDTO>> GetAllAsync()
        {
            var cartitems = await repositorywrapper.CartItemRepository.GetAllAsync();
            return mapper.Map<IEnumerable<CartItemResponseDTO>>(cartitems);

        }

        public async Task<CartItemResponseDTO> GetByIdAsync(int id)
        {
            var cartItem = await repositorywrapper.CartItemRepository.GetByIdAsync(id);
            return mapper.Map<CartItemResponseDTO>(cartItem);

        }

        public async Task<CartItemResponseDTO> CreateAsync(CartItemRequestDTO requestdto) {

            var cartitem = mapper.Map<CartItem>(requestdto);
            var createdcartitem = await repositorywrapper.CartItemRepository.CreateAsync(cartitem);
            return  mapper.Map<CartItemResponseDTO>(createdcartitem);
        
        }



        public async Task<CartItemResponseDTO> UpdateAsync(int id, CartItemRequestDTO requestdto)
        {
            var cartitem = mapper.Map<CartItem>(requestdto);
            var updatedcartitem = await repositorywrapper.CartItemRepository.UpdateAsync(id, cartitem);
            return mapper.Map<CartItemResponseDTO>(updatedcartitem);
        }

        public async Task DeleteAsync(int id)
        {
            var cartitem = await repositorywrapper.CartItemRepository.GetByIdAsync(id);
            if (cartitem != null)
            {
                await repositorywrapper.CartItemRepository.DeleteAsync(cartitem);
            }
        }

    }
}
