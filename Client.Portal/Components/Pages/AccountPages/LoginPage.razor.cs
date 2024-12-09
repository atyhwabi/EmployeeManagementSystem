using BaseLibrary.DTOs;
using ClientLibrary.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using ClientLibrary.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Client.Portal.Components.Pages.AccountPages
{
    public partial class LoginPage
    {
        private Login User = new Login();
        private bool isPrerendering;
        private async Task HandleLogin()
        {
            try
            {
                var response = await UserAccountService.LoginAsync(User);
                if (response.Flag)
                {
                    var customerUserClaims = (CustomAuthenticationProvider)AuthStateProvider;
                    await customerUserClaims.UpdateAuthenticationState(new UserSession() { Token = response.Token, RefreshToken = response.RefreshToken });
                    NavManager.NavigateTo("/", forceLoad: true);
                }
            }
            catch( Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        protected override Task OnInitializedAsync()
        {
            isPrerendering = NavManager.Uri.Contains("prerendering");
            return base.OnInitializedAsync();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && !isPrerendering)
            {
                await JSRuntime.InvokeVoidAsync("console.log", "JS interop called after rendering!");
            }
        }
    }
}
