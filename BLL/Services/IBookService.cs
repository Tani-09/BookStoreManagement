using AutoMapper;
using BookStoreManagement.BLL.DTO.RequestDTO;
using BookStoreManagement.BLL.DTO.ResponseDTO;
using BookStoreManagement.DAL.Entities;
using BookStoreManagement.DAL.Repositories;

namespace BookStoreManagement.BLL.Services
{
    public interface IBookService : IBaseService<BookRequestDTO, BookResponseDTO>
    {
       
    }

    public class BookService : IBookService {

        private readonly IRepositoryWrapper _repositorywrapper;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;

        public BookService(IRepositoryWrapper repositorywrapper, IMapper mapper, ILogger<BookService> logger)
        {
            _repositorywrapper = repositorywrapper;
            _mapper = mapper;
            _logger = logger;
        }  
        
        public async Task<IEnumerable<BookResponseDTO>> GetAllAsync()
        {

            _logger.LogInformation("Fetching all books.");
            var books = await _repositorywrapper.BookRepository.GetAllAsync();
            _logger.LogInformation($"Fetched {books.Count()} books.");
            return _mapper.Map<IEnumerable<BookResponseDTO>>(books);
            
        }

        public async Task<BookResponseDTO> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching book with ID {id}.");
                var book = await _repositorywrapper.BookRepository.GetByIdAsync(id);
                if (book == null)
                {
                    _logger.LogWarning($"Book with ID {id} not found.");
                    return null;
                }
                _logger.LogInformation($"Fetched book: {book.Title}");
                return _mapper.Map<BookResponseDTO>(book);
            }

            catch(Exception ex) 
            {
                _logger.LogError(ex, $"Error fetching book with ID {id}");
                throw;
            }
           

           // _logger.LogInformation($"Fetched book: {book.Title}");
           // return _mapper.Map<BookResponseDTO>(book);
        }

        public async Task<BookResponseDTO> CreateAsync(BookRequestDTO requestdto)
        {
            var book = _mapper.Map<Book>(requestdto);

            _logger.LogInformation($"Creating a new book: {requestdto.Title}");
            var createdbook = await _repositorywrapper.BookRepository.CreateAsync(book);

            _logger.LogInformation($"Created book: {createdbook.Title}");
            return _mapper.Map<BookResponseDTO>(createdbook);
        }

        public async Task<BookResponseDTO> UpdateAsync(int id, BookRequestDTO requestdto)
        {
            var book = _mapper.Map<Book>(requestdto);
            var updatedbook =await  _repositorywrapper.BookRepository.UpdateAsync(id, book);
            return _mapper.Map<BookResponseDTO>(updatedbook);
        }

        public async Task DeleteAsync(int id)
        {
            var book = await _repositorywrapper.BookRepository.GetByIdAsync(id);
            if (book != null)
            {
                await _repositorywrapper.BookRepository.DeleteAsync(book);
            }
        }
    }
}
