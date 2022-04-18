namespace Project.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
    }
}
