using Models.DtoModels;

namespace Repository
{
    public interface ITokenRepo
    {
        bool IsTokenAboutToExpire(DateTime tokenExpiration);
        Task<UserDTO> RefreshTokenAsync(UserDTO person);
        Task<UserDTO> TokenValidationPackageAsync(UserDTO cookie);
        Task<bool> ValidateTokenAsync(string token);
    }
}