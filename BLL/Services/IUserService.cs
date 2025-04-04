using AutoMapper;
using BookStoreManagement.BLL.DTO.RequestDTO;
using BookStoreManagement.BLL.DTO.ResponseDTO;
using BookStoreManagement.DAL.DBContext;
using BookStoreManagement.DAL.Entities;
using BookStoreManagement.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace BookStoreManagement.BLL.Services
{
    public interface IUserService: IBaseService<UserRequestDTO, UserResponseDTO>
    {
        Task<User> GetUserByUsernameAsync(string username);
    }

    public class UserService: IUserService
    {
        private readonly IRepositoryWrapper repositorywrapper;
        private readonly IMapper mapper;
        private readonly BookStoreDbContext _context;

        public UserService(IRepositoryWrapper repositorywrapper, IMapper mapper, BookStoreDbContext context)
        {
            this.repositorywrapper = repositorywrapper;
            this.mapper = mapper;
            _context = context;
        }


        public async Task<User> GetUserByUsernameAsync(string username)
        {
           
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == username);
        }



        public async Task<IEnumerable<UserResponseDTO>> GetAllAsync()
        {
            var users = await repositorywrapper.UserRepository.GetAllAsync();
            return mapper.Map<IEnumerable<UserResponseDTO>>(users);

        }

        public async Task<UserResponseDTO> GetByIdAsync(int id)
        {
            var user = await repositorywrapper.UserRepository.GetByIdAsync(id);
            return mapper.Map<UserResponseDTO>(user);
        }

        public async Task<UserResponseDTO> CreateAsync(UserRequestDTO requestdto)
        {
            var user = mapper.Map<User>(requestdto);

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            var role = await _context.Roles.FindAsync(requestdto.RoleId);
            if (role == null)
            {
                throw new ArgumentException("Invalid role ID");
            }
            user.Role = role;

            var createduser = await repositorywrapper.UserRepository.CreateAsync(user);
            return mapper.Map<UserResponseDTO>(createduser);
        }

        public async Task<UserResponseDTO> UpdateAsync(int id, UserRequestDTO requestdto)
        {
            var existingUser = await repositorywrapper.UserRepository.GetByIdAsync(id);

            var user = mapper.Map<User>(requestdto);

            if (!string.IsNullOrEmpty(requestdto.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(requestdto.Password);
            }


            if (requestdto.RoleId != existingUser.RoleId)
            {
                var role = await _context.Roles.FindAsync(requestdto.RoleId);
                if (role == null)
                {
                    throw new KeyNotFoundException($"Role with id {requestdto.RoleId} not found.");
                }
                existingUser.RoleId = role.Id;
                existingUser.Role = role;
            }

            var updateduser = await repositorywrapper.UserRepository.UpdateAsync(id, user);
            return mapper.Map<UserResponseDTO>(updateduser);
        }

        public async Task DeleteAsync(int id)
        {
            var user = await repositorywrapper.UserRepository.GetByIdAsync(id);
            if (user != null)
            {
                await repositorywrapper.UserRepository.DeleteAsync(user);
            }
        }

    } 
}
