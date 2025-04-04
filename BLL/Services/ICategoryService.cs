using AutoMapper;
using BookStoreManagement.BLL.DTO.RequestDTO;
using BookStoreManagement.BLL.DTO.ResponseDTO;
using BookStoreManagement.DAL.Entities;
using BookStoreManagement.DAL.Repositories;

namespace BookStoreManagement.BLL.Services
{
    public interface ICategoryService: IBaseService<CategoryRequestDTO, CategoryResponseDTO>
    {


    }

    public class CategoryService: ICategoryService
    {
        private readonly IRepositoryWrapper repositorywrapper;
        private readonly IMapper mapper;

        public CategoryService(IRepositoryWrapper repositorywrapper, IMapper mapper)
        {
            this.repositorywrapper = repositorywrapper;
            this.mapper = mapper;
        }




        public async Task<IEnumerable<CategoryResponseDTO>> GetAllAsync()
        {
            var category = await repositorywrapper.CategoryRepository.GetAllAsync();
            return mapper.Map<IEnumerable<CategoryResponseDTO>>(category);

        }

        public async Task<CategoryResponseDTO> GetByIdAsync(int id)
        {
            var category = await repositorywrapper.CategoryRepository.GetByIdAsync(id);
            return mapper.Map<CategoryResponseDTO>(category);
        }

        public async Task<CategoryResponseDTO> CreateAsync(CategoryRequestDTO requestdto)
        {
            var category = mapper.Map<Category>(requestdto);
            var createdcategory = await repositorywrapper.CategoryRepository.CreateAsync(category);
            return mapper.Map<CategoryResponseDTO>(createdcategory);
        }

        public async Task<CategoryResponseDTO> UpdateAsync(int id, CategoryRequestDTO requestdto)
        {
            var category = mapper.Map<Category>(requestdto);
            var updatedcategory =   await repositorywrapper.CategoryRepository.UpdateAsync(id, category);
            return mapper.Map<CategoryResponseDTO>(updatedcategory);
        }

        public async Task DeleteAsync(int id)
        {
            var category = await repositorywrapper.CategoryRepository.GetByIdAsync(id);
            if (category != null)
            {
                await repositorywrapper.CategoryRepository.DeleteAsync(category);
            }
        }
    }
}
