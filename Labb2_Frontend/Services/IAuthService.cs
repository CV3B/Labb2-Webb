using Labb2_Shared.DTO;

namespace Labb2_Frontend.Services;

public interface IAuthService
{
    Task<bool> RegisterCustomer(CustomerDTO request);
    Task<bool> LoginCustomer(LoginDTO request);
    Task Logout();
}