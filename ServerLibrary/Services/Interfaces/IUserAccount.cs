using BaseLibrary.DTOs;
using BaseLibrary.Entities;
using BaseLibrary.Responses;

namespace ServerLibrary.Services.Interfaces
{
    public interface IUserAccount
    {
        Task<GeneralResponse> CreateAsync(Register user);
        Task<LoginResponse> LoginAsync(Login user);
        Task<LoginResponse> RefreshTokenAsync(RefreshToken token);

    }
}
