using System.Net.Http.Json;
using Blazored.LocalStorage;
using Labb2_Frontend.Providers;
using Labb2_Shared.DTO;

namespace Labb2_Frontend.Services;

public class AuthService(
    HttpClient httpClient,
    ILocalStorageService localStorage,
    AuthStateProvider authStateProvider)
    : IAuthService
{
    public async Task<bool> RegisterCustomer(CustomerDTO request)
    {
        var response = await httpClient.PostAsJsonAsync("auth/register", request);

        authStateProvider.NotifyCustomerAuthentication();
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> LoginCustomer(LoginDTO request)
    {
        var response = await httpClient.PostAsJsonAsync("auth/login", request);

        if (!response.IsSuccessStatusCode) return false;

        var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponseDTO>();

        if (tokenResponse == null) return false;

        await localStorage.SetItemAsync("accessToken", tokenResponse.AccessToken);
        await localStorage.SetItemAsync("refreshToken", tokenResponse.RefreshToken);

        await authStateProvider.AttachToken();
        authStateProvider.NotifyCustomerAuthentication();

        return true;
    }

    public async Task Logout()
    {
        await localStorage.RemoveItemAsync("accessToken");
        await localStorage.RemoveItemAsync("refreshToken");

        httpClient.DefaultRequestHeaders.Authorization = null;
        authStateProvider.NotifyCustomerAuthentication();
    }
}