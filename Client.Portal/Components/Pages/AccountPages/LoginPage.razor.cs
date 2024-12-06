using BaseLibrary.DTOs;
using ClientLibrary.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using ClientLibrary.Services.Interfaces;


namespace Client.Portal.Components.Pages.AccountPages
{
    public partial class LoginPage
    {
        private Login User = new Login();
        private async Task HandleLogin()
        {
            var response = await UserAccountService.LoginAsync(User);
            if (response.Flag)
            {
                var customerUserClaims = (CustomAuthenticationProvider)AuthStateProvider;
                await customerUserClaims.UpdateAuthenticationState(new UserSession() { Token = response.Token, RefreshToken = response.RefreshToken });
                NavManager.NavigateTo("/", forceLoad: true);
            }
        }
        protected override void OnInitialized()
        {
          
            base.OnInitialized();
        }
    }
}
