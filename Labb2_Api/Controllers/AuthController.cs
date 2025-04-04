using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Labb2_Api.Data;
using Labb2_Shared.DTO;
using Labb2_Shared.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Labb2_Api.Controllers;

[Route("auth")]
[ApiController]
public class AuthController(IConfiguration config, IUnitOfWork unitOfWork) : ControllerBase
{
    /// <summary>
    /// Register a new customer
    /// </summary>
    /// <param name="request">CustomerDTO is the data of the new customer</param>
    /// <returns>Returns the registered customer</returns>
    [HttpPost("register")]
    public async Task<ActionResult<Customer>> Register(CustomerDTO request)
    {
        var customer = new Customer();
        var hashedPassword = new PasswordHasher<Customer>()
            .HashPassword(customer, request.Password);

        customer.Email = request.Email;
        customer.FirstName = request.FirstName;
        customer.LastName = request.LastName;
        customer.Phone = request.PhoneNumber;
        customer.Address = request.Address;
        customer.PasswordHash = hashedPassword;

        await unitOfWork.CustomerRepository.Add(customer);
        await unitOfWork.Save();

        return Ok(customer);
    }

    /// <summary>
    /// Login with a customer
    /// </summary>
    /// <param name="request">LoginDTO is the data of the customer login</param>
    /// <returns>Returns the customer jwt token and refresh token</returns>
    [HttpPost("login")]
    public async Task<ActionResult<TokenResponseDTO>> Login(LoginDTO request)
    {
        var customer = await unitOfWork.CustomerRepository.GetByEmail(request.Email);
        if (new PasswordHasher<Customer>().VerifyHashedPassword(customer, customer.PasswordHash, request.Password)
            == PasswordVerificationResult.Failed)
        {
            return BadRequest("Invalid credentials");
        }

        if (customer is null)
            return BadRequest("Invalid username or password.");

        return Ok(await CreateTokenResponse(customer));
    }

    /// <summary>
    /// Refreshes the tokens of the customer
    /// </summary>
    /// <param name="request">RefreshTokenRequestDTO data of the user id and refresh token</param>
    /// <returns>Returns new tokens</returns>
    [HttpPost("refresh-token")]
    public async Task<ActionResult<TokenResponseDTO>> RefreshToken(RefreshTokenRequestDTO request)
    {
        var result = await ValidateRefreshToken(request.UserId, request.RefreshToken);

        if (result is null || result.RefreshToken is null)
            return Unauthorized("Invalid refresh token.");

        return Ok(await CreateTokenResponse(result));
    }

    private async Task<TokenResponseDTO> CreateTokenResponse(Customer? customer)
    {
        return new TokenResponseDTO
        {
            AccessToken = CreateToken(customer),
            RefreshToken = await GenerateAndSaveRefreshToken(customer)
        };
    }

    private async Task<Customer?> ValidateRefreshToken(Guid userId, string refreshToken)
    {
        var customer = await unitOfWork.CustomerRepository.GetById(userId);
        if (customer is null || customer.RefreshToken != refreshToken
                             || customer.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return null;
        }

        return customer;
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        
        return Convert.ToBase64String(randomNumber);
    }

    private async Task<string> GenerateAndSaveRefreshToken(Customer customer)
    {
        var refreshToken = GenerateRefreshToken();
        customer.RefreshToken = refreshToken;
        customer.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        
        await unitOfWork.Save();
        
        return refreshToken;
    }

    private string CreateToken(Customer customer)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, customer.Email),
            new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString()),
            new Claim(ClaimTypes.Role, customer.Role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config.GetValue<string>("AppSettings:Token")!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: config.GetValue<string>("AppSettings:Issuer"),
            audience: config.GetValue<string>("AppSettings:Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}