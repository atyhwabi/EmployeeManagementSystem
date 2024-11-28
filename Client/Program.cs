using Blazored.LocalStorage;
using Client.Components;
using ClientLibrary.Helpers;
using ClientLibrary.Services.Implementations;
using ClientLibrary.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.  
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7121") });
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();  // Ensure this is correctly registered  
builder.Services.AddScoped<GetHttpClient>();
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationProvider>();
builder.Services.AddScoped<IUserAccountService, UserAccountService>();

var app = builder.Build();

// Configure the HTTP request pipeline.  
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Ensure you use this middleware for authorization if needed  
app.UseAuthentication();
app.UseAuthorization();

// Set up routing to your main app component  
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();