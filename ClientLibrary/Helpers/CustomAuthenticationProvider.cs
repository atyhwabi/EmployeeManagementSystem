using BaseLibrary.DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.IO.Pipes;
using System.Security.Claims;
namespace ClientLibrary.Helpers
{
    public class CustomAuthenticationProvider(LocalStorageService localStorageService) : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var stringToken = await localStorageService.GetToken();
            if (string.IsNullOrEmpty(stringToken)) return await Task.FromResult(new AuthenticationState(anonymous));

            var deserializedToken = Serializations.Deserialize<UserSession>(stringToken);
            if (deserializedToken == null) return await Task.FromResult(new AuthenticationState(anonymous));

            var getUserClaims = DecrytToken(deserializedToken.Token);
            if (getUserClaims == null) return await Task.FromResult(new AuthenticationState(anonymous));

            var claimsPrincipal = SetClaimPrincipal(getUserClaims);
            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }
        public static ClaimsPrincipal SetClaimPrincipal(CustomerUserClaims userClaims)
        {
            if (userClaims == null) return new ClaimsPrincipal(new ClaimsIdentity());
            if (string.IsNullOrEmpty(userClaims.Email)) return new ClaimsPrincipal(new ClaimsIdentity());

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userClaims.Id),
                new Claim(ClaimTypes.Email, userClaims.Email),
                new Claim(ClaimTypes.Name, userClaims.Name),
                new Claim(ClaimTypes.Role, userClaims.Role)
            };

            var identity = new ClaimsIdentity(claims, "JwtAuth");
            return new ClaimsPrincipal(identity);
        }
        public async Task UpdateAuthenticationState(UserSession userSession)
        {
            var claimsPrincipal = new ClaimsPrincipal();

            if (userSession.Token != null || userSession.RefreshToken != null)
            {
                var serializeSession = Serializations.Serialize(userSession);
                await localStorageService.SetToken(serializeSession);
                var getUserClaims = DecrytToken(userSession.Token!);
                claimsPrincipal = SetClaimPrincipal(getUserClaims);
            }
            else
            {
                await localStorageService.RemoveToken();
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
        public static CustomerUserClaims DecrytToken(string jwtToken)
        {
            if (string.IsNullOrEmpty(jwtToken)) return new CustomerUserClaims();

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);

            var userId = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.NameIdentifier);
            var email = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Email);
            var fullname = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Name);
            var role = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Role);

            return new CustomerUserClaims()
            {
                Id = userId!.Value,
                Email = email!.Value,
                Name = fullname!.Value,
                Role = role!.Value
            };
        }
    }
}
