using domain.Models;

namespace core.Dtos
{
    public class UserLoginResponseDto
    {
        public int UserId { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}
