using BaseLibrary.DTOs;
using BaseLibrary.Entities;
using ServerLibrary.Helpers;
using BaseLibrary.Responses;
using Microsoft.Extensions.Options;
using ServerLibrary.Data;
using ServerLibrary.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;



namespace ServerLibrary.Services.Implementations
{
    public class UserAccountRepository(IOptions<JwtSection> config, AppDBContext appContext) : IUserAccount
    {
        public async Task<GeneralResponse> CreateAsync(Register user)
        {
            if (user == null) return new GeneralResponse(false, "User cannot be null");

            if (await UserExists(user.Email)) return new GeneralResponse(false, "User already exists");

            var applicationUser = await AddToDatabase(new ApplicationUser()
            {
                Fullname = user.Fullname,
                Email = user.Email,
                Password =  BCrypt.Net.BCrypt.HashPassword(user.Password)

            });

            var checkAdminRoile = await appContext.SystemRoles.FirstOrDefaultAsync(_ => _.RoleName!.Equals(Constants.Admin));
            if (checkAdminRoile == null)
            {
                var ceateAdmiRole = await AddToDatabase(new SystemRole()
                {
                    RoleName = Constants.Admin
                });

                await AddToDatabase(new UserRole()
                {
                    UserId = applicationUser.Id,
                    RoleId = ceateAdmiRole.Id
                });
                return new GeneralResponse(true, "User created successfully");
            }

            var checkUserRoile = await appContext.SystemRoles.FirstOrDefaultAsync(_ => _.RoleName!.Equals(Constants.User));
            SystemRole reponse = new();
            if (checkAdminRoile is null)
            {
                reponse = await AddToDatabase(new SystemRole()
                {
                    RoleName = Constants.User
                });
            }
            else
            {
                await AddToDatabase(new UserRole()
                {
                    UserId = applicationUser.Id,
                    RoleId = reponse.Id
                });
            }

            return new GeneralResponse(true, "User created successfully");
        }

        public  async Task<LoginResponse> LoginAsync(Login user)
        {
           var applicationUser = appContext.ApplicationUsers.FirstOrDefaultAsync(x => x.Email == user.Email);
            if (applicationUser == null) return new LoginResponse(false, "User does not exist");

            if (!BCrypt.Net.BCrypt.Verify(user.Password, applicationUser.Result!.Password))
            {
                return new LoginResponse(false, "Invalid password/email");
            }

            var getUserRoles = await FindUserRole(applicationUser.Result!.Id);
            if (getUserRoles is null ) return new LoginResponse(false, "User has no role");

            var getUserRoleName = await FindRole(getUserRoles.RoleId);
            if (getUserRoleName is null) return new LoginResponse(false, "User has no role");

            string jwtToken = GenerateJwtToken(applicationUser.Result, getUserRoleName.RoleName);
            string refreshToken = GenerateRefreshToken();

            var findToken = await appContext.RefreshTokenInfos.FirstOrDefaultAsync(x => x.UserId == applicationUser.Result.Id);
            if (findToken is null)
            {
                await AddToDatabase(new RefreshTokenInfo()
                {
                    UserId = applicationUser.Result.Id,
                    Token = refreshToken
                });
            }
            else
            {
                findToken.Token = refreshToken;
                await appContext.SaveChangesAsync();
            }
            return new LoginResponse(true, "Login successful", jwtToken, refreshToken);
        }
        public string GenerateJwtToken(ApplicationUser user, string role)
        {
            var tokenHandler = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Value.Key!));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Value.Key!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.Fullname!)
            };

             
            var token = new JwtSecurityToken(
                  issuer: config.Value.Issuer,
                  audience: config.Value.Audience,
                  claims: userClaims,
                  expires: DateTime.Now.AddDays(1),
                  signingCredentials: credentials
             );
            return token.EncodedPayload;

        }

        private async Task<UserRole> FindUserRole(int userId)
        {
            return await appContext.UserRoles.FirstOrDefaultAsync(x => x.UserId == userId);
        }
        private async Task<SystemRole> FindRole(int roleId)
        {
            return await appContext.SystemRoles.FirstOrDefaultAsync(x => x.Id == roleId);
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        public async Task<bool> UserExists(string email)
        {
            return await appContext.ApplicationUsers.AnyAsync(x => x.Email == email);
        }
        public async Task<T> AddToDatabase<T>(T Model)
        {
            var result =  appContext.Add(Model!);
            await appContext.SaveChangesAsync();
            return (T)result.Entity;
        }
        public async Task<LoginResponse> RefreshTokenAsync(RefreshToken token)
        {
            if(token == null) return new LoginResponse(false, "Token cannot be null");


            var findToken = await appContext.RefreshTokenInfos.FirstOrDefaultAsync(x => x.Token == token.Token);
            if (findToken == null) return new LoginResponse(false, "Token does not exist");


            var user = await appContext.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == findToken!.UserId);

            if (user == null) return new LoginResponse(false, "User does not exist");

            var getUserRoles = await FindUserRole(user.Id);
            if (getUserRoles is null) return new LoginResponse(false, "User has no role");

            var getUserRoleName = await FindRole(getUserRoles.RoleId);
            if (getUserRoleName is null) return new LoginResponse(false, "User has no role");

            string jwtToken = GenerateJwtToken(user, getUserRoleName.RoleName);
            string refreshToken = GenerateRefreshToken();

            var updateToken = await appContext.RefreshTokenInfos.FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (updateToken is null) return new LoginResponse(false, "Token does not exist");

            updateToken.Token = refreshToken;
            await appContext.SaveChangesAsync();

            return new LoginResponse(true, "Login successful", jwtToken, refreshToken);
        }   
    }
}
