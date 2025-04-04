namespace Labb2_Shared.DTO;

public class RefreshTokenRequestDTO
{
    public Guid UserId { get; set; }
    public required string RefreshToken { get; set; }
}