﻿namespace BookStoreManagement.BLL.DTO
{
    public class UserLoginDTO
    {

        public required string Username { get; set; }
        public required string Password { get; set; }

        public required string Role { get; set; }
    }
}
