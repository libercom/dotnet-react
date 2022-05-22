using domain.Models;
using System.Text.Json.Serialization;

namespace core.Dtos
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
        [JsonIgnore]
        public string Password { get; set; }
    }
}
