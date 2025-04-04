namespace Labb2_Shared.Model
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        
        public string PasswordHash { get; set; }
        
        public string Role { get; set; } = "Customer";
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        
        public string FullName => $"{FirstName} {LastName}";
    }
}
