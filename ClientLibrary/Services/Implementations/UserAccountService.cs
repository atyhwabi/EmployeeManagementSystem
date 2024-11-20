using BaseLibrary.DTOs;
using BaseLibrary.Responses;
using ClientLibrary.Helpers;
using ClientLibrary.Services.Interfaces;
using System.Net.Http.Json;


namespace ClientLibrary.Services.Implementations
{
    public class UserAccountService(GetHttpClient getHttpClient) : IUserAccountService
    {
        public const string authPath = "api/authentication";
        public async Task<GeneralResponse> CreateAsync(Register user)
        {
            var httpClient = await getHttpClient.GetPrivateHttpClient();
            var response = await httpClient.PostAsJsonAsync($"{authPath}/register", user);

            return await response.Content.ReadFromJsonAsync<GeneralResponse>()!;
        }

        public async Task<LoginResponse> LoginAsync(Login user)
        {
            var httpClient = await getHttpClient.GetPrivateHttpClient();
            var response = await httpClient.PostAsJsonAsync($"{authPath}/login", user);
            if(!response.IsSuccessStatusCode) return new LoginResponse(false, "Invalid login credentials");

            return await response.Content.ReadFromJsonAsync<LoginResponse>()!;
        }

        public Task<LoginResponse> RefreshTokenAsync(RefreshToken token)
        {
            throw new NotImplementedException();
        }
    }
}
