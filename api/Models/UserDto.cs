﻿namespace Project.Models
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Company Company { get; set; }
        public Role Role { get; set; }
        public Country Country { get; set; }
    }
}
