using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace Labb2_Frontend.Providers;

public class AuthStateProvider(HttpClient httpClient, ISessionStorageService sessionStorage)
    : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await sessionStorage.GetItemAsync<string>("accessToken");

        var identity = string.IsNullOrEmpty(token)
            ? new ClaimsIdentity()
            : new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");

        var user = new ClaimsPrincipal(identity);
        return new AuthenticationState(user);
    }

    public void NotifyCustomerAuthentication()
    {
     NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        return jwt.Claims;
    }

    public async Task AttachToken()
    {
        var token = await sessionStorage.GetItemAsync<string>("accessToken");

        if (!string.IsNullOrEmpty(token))
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}