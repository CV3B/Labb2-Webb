using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Labb2_Frontend;
using Labb2_Frontend.Providers;
using Labb2_Frontend.Services;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredSessionStorage();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddScoped<AuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<AuthStateProvider>());


builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["Api:Url"]) });

builder.Services.AddMudServices();

await builder.Build().RunAsync();