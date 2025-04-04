using BookStoreManagement.BLL.DTO.RequestDTO;
using BookStoreManagement.DAL.DBContext;
using BookStoreManagement.DAL.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BookStoreManagement.DAL.Repositories
{

    //Making Interface for containing all the method definitions
    public interface IBaseRepository<T> where T : class
    {
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(int id, T entity);
        Task DeleteAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
    }


    //REPOSITORY CLASS TO IMPLEMENT ALL BASE METHODS DECLARED IN INTERFACE
    public class RepositoryBase<T> : IBaseRepository<T> where T : class
    {
        private readonly BookStoreDbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        public RepositoryBase(BookStoreDbContext _dbContext)
        {
            this._dbContext = _dbContext;
            this._dbSet = this._dbContext.Set<T>();
        }
        /* public async Task<T> CreateAsync(T entity)
           {
               var result = await _dbSet.AddAsync(entity);
               await _dbContext.SaveChangesAsync();
               return result.Entity;
           }  */

        public async Task<T> CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {

            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();

        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {

            //return await _dbSet.FindAsync(id);
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with id {id} not found.");
            }
            return entity;

        }

        public async Task<T> UpdateAsync(int id, T entity)
        {
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;


        }
    }



    //MAKING INDIVIDUAL REPOSITORY INTERFACES AND THEIR CLASSES


    public interface IBookRepository : IBaseRepository<Book> { };
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(BookStoreDbContext _dbContext) : base(_dbContext)
        {
        }
    }


    public interface ICartItemRepository : IBaseRepository<CartItem> { };
    public class CartItemRepository : RepositoryBase<CartItem>, ICartItemRepository
    {
        public CartItemRepository(BookStoreDbContext _dbContext) : base(_dbContext)
        {
        }
    }


    public interface ICategoryRepository : IBaseRepository<Category> { };
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(BookStoreDbContext _dbContext) : base(_dbContext)
        {
        }
    }


    //CHANGES


    public interface IOrderRepository : IBaseRepository<Order> {

        Task<Order> CreateOrderAsync(OrderRequestDTO orderRequest);
        Task<Order> GetOrderAsync(int id);
        

    };
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {

        private readonly BookStoreDbContext _dbcontext;
        public OrderRepository(BookStoreDbContext dbContext) : base(dbContext)
        {

            _dbcontext = dbContext;
        }


        public async Task<Order> GetOrderAsync(int id)
        {
            return await _dbcontext.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .FirstOrDefaultAsync(o => o.Id == id);
        }



        public async Task<Order> CreateOrderAsync(OrderRequestDTO orderRequest)
        {
            var orderItems = new List<OrderItem>();

            foreach (var item in orderRequest.OrderItems)
            {
                var book = await _dbcontext.Books.FindAsync(item.BookId);
                if (book != null)
                {
                    orderItems.Add(new OrderItem
                    {
                        BookId = item.BookId,
                        Quantity = item.Quantity,
                        Price = book.Price
                    });
                }
            }

            foreach (var orderItem in orderItems)
            {
                Console.WriteLine($"BookId: {orderItem.BookId}, Quantity: {orderItem.Quantity}, Price: {orderItem.Price}");
            }

            decimal totalAmount = orderItems.Sum(item => item.Price * item.Quantity);
            Console.WriteLine($"Total Amount Calculated: {totalAmount}");

            var order = new Order
            {
                UserId = orderRequest.UserId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                OrderItems = orderItems
            };

            _dbcontext.Orders.Add(order);
            await _dbcontext.SaveChangesAsync();

            return order;
        }

       
    }




    //CHANGES

    public interface IOrderItemRepository : IBaseRepository<OrderItem> { };
    public class OrderItemRepository : RepositoryBase<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(BookStoreDbContext _dbContext) : base(_dbContext)
        {
        }
    }


    public interface IRoleRepository : IBaseRepository<Role> { };
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(BookStoreDbContext _dbContext) : base(_dbContext)
        {
        }
    }


    public interface IUserRepository : IBaseRepository<User> { };
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(BookStoreDbContext _dbContext) : base(_dbContext)
        {
        }
    }


    //REPOSITORY WRAPPER




    public interface IRepositoryWrapper
    {
        IBookRepository BookRepository { get; }
        IOrderRepository OrderRepository { get; }
        ICartItemRepository CartItemRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IOrderItemRepository OrderItemRepository { get; }
        IRoleRepository RoleRepository { get; }

        IUserRepository UserRepository { get; }
    }

    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly BookStoreDbContext _context;

        private IBookRepository? _bookRepository;
        private IOrderRepository? _orderRepository;
        private ICartItemRepository? _cartItemRepository;
        private ICategoryRepository? _categoryRepository;
        private IOrderItemRepository? _orderItemRepository;
        private IRoleRepository? _roleRepository;
        private IUserRepository? _userRepository;

        public RepositoryWrapper(BookStoreDbContext context)
        {
            _context = context;
          

        }



        public IBookRepository BookRepository
        {
            get
            {
                if (_bookRepository == null)
                {
                    _bookRepository = new BookRepository(_context);
                }
                return _bookRepository;
            }
        }

        public IOrderRepository OrderRepository
        {
            get
            {
                if (_orderRepository == null)
                {
                    _orderRepository = new OrderRepository(_context);
                }
                return _orderRepository;
            }
        }

        public ICartItemRepository CartItemRepository
        {
            get
            {
                if (_cartItemRepository == null)
                {
                    _cartItemRepository = new CartItemRepository(_context);
                }
                return _cartItemRepository;
            }
        }

        public ICategoryRepository CategoryRepository
        {
            get
            {
                if (_categoryRepository == null)
                {
                    _categoryRepository = new CategoryRepository(_context);
                }
                return _categoryRepository;
            }
        }

        public IOrderItemRepository OrderItemRepository
        {
            get
            {
                if (_orderItemRepository == null)
                {
                    _orderItemRepository = new OrderItemRepository(_context);
                }
                return _orderItemRepository;
            }
        }

        public IRoleRepository RoleRepository
        {
            get
            {
                if (_roleRepository == null)
                {
                    _roleRepository = new RoleRepository(_context);
                }
                return _roleRepository;
            }
        }


        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_context);
                }
                return _userRepository;
            }
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }







    }

}
