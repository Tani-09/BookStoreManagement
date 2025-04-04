namespace BookStoreManagement.DAL.Entities
{
    public class User
    {

        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }

        public int RoleId { get; set; }  // Added RoleId as foreign key for the relationship
        public Role? Role { get; set; }

        public ICollection<Order>? Orders { get; set; }
        public ICollection<CartItem>? CartItems { get; set; }

    }
}
