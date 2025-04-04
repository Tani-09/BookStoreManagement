using AutoMapper;
using BookStoreManagement.BLL.DTO.RequestDTO;
using BookStoreManagement.BLL.DTO.ResponseDTO;
using BookStoreManagement.DAL.Entities;
using BookStoreManagement.DAL.Repositories;

namespace BookStoreManagement.BLL.Services
{
    public interface IRoleService: IBaseService<RoleRequestDTO, RoleResponseDTO>
    {
   
    }

    public class RoleService : IRoleService {

        private readonly IRepositoryWrapper repositorywrapper;
        private readonly IMapper mapper;

        public RoleService(IRepositoryWrapper repositorywrapper, IMapper mapper)
        {
            this.repositorywrapper = repositorywrapper;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<RoleResponseDTO>> GetAllAsync()
        {
            var roles = await repositorywrapper.RoleRepository.GetAllAsync();
            return mapper.Map<IEnumerable<RoleResponseDTO>>(roles);

        }

        public async Task<RoleResponseDTO> GetByIdAsync(int id)
        {
            var role = await repositorywrapper.RoleRepository.GetByIdAsync(id);
            return mapper.Map<RoleResponseDTO>(role);
        }

        public async Task<RoleResponseDTO> CreateAsync(RoleRequestDTO requestdto)
        {
            var role = mapper.Map<Role>(requestdto);
            var createdrole = await repositorywrapper.RoleRepository.CreateAsync(role);
            return mapper.Map<RoleResponseDTO>(createdrole);
        }

        public async Task<RoleResponseDTO> UpdateAsync(int id, RoleRequestDTO requestdto)
        {
            var role = mapper.Map<Role>(requestdto);
            var updatedrole = await repositorywrapper.RoleRepository.UpdateAsync(id, role);
            return mapper.Map<RoleResponseDTO>(updatedrole);
        }

        public async Task DeleteAsync(int id)
        {
            var role = await repositorywrapper.RoleRepository.GetByIdAsync(id);
            if (role != null)
            {
                await repositorywrapper.RoleRepository.DeleteAsync(role);
            }
        }

    }
}
