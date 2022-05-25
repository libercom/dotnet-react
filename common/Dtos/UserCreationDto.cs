namespace common.Dtos
{
    public class UserCreationDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int CompanyId { get; set; }
        public int RoleId { get; set; }
        public int CountryId { get; set; }
        public string Password { get; set; }
    }
}
