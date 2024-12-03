using Blazored.LocalStorage;
using Client.Portal.Components;
using ClientLibrary.Helpers;
using ClientLibrary.Services.Implementations;
using ClientLibrary.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
//builder.Services.AddAuthorizationCore();
//builder.Services.AddBlazoredLocalStorage();
//builder.Services.AddScoped<GetHttpClient>();
//builder.Services.AddScoped<AuthenticationStateProvider,CustomAuthenticationProvider>();
//builder.Services.AddScoped<LocalStorageService>();
//builder.Services.AddScoped<IUserAccountService, UserAccountService>();
//await builder.Build().RunAsync();

builder.Services.AddAuthenticationCore();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
