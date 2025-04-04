using AutoMapper;
using BookStoreManagement.BLL.DTO.RequestDTO;
using BookStoreManagement.BLL.DTO.ResponseDTO;
using BookStoreManagement.DAL.Entities;


namespace BookStoreManagement.BLL.AutoMapper
{
    public class MappingProfile: Profile
    {
        public MappingProfile() {




            CreateMap<Order, OrderResponseDTO>();
            CreateMap<OrderRequestDTO, Order>();


            CreateMap<Book, BookResponseDTO>();
            CreateMap<BookRequestDTO, Book>();


            CreateMap<CartItem, CartItemResponseDTO>();
            CreateMap<CartItemRequestDTO, CartItem>();


            CreateMap<Category, CategoryResponseDTO>();
            CreateMap<CategoryRequestDTO, Category>();


            CreateMap<OrderItem, OrderItemResponseDTO>();
            CreateMap<OrderItemRequestDTO, OrderItem>();

            CreateMap<Role, RoleResponseDTO>();
            CreateMap<RoleRequestDTO, Role>();

            CreateMap<User, UserResponseDTO>();
            CreateMap<UserRequestDTO, User>();





        }
       

    }
}
