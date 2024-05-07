using Models.DtoModels;
using Models.EntityModels;
using Models.Forms;

namespace Repo
{
    public interface IAccountRepo
    {
        Task<UserDTO> CreateAccountAsync(UserCreateForm userDto);
        Task<bool> DoesUserExistAsync(User user);
        Task<bool> DoesUserExistAsync(UserDTO user);
        Task<byte[]> GetSecretKeyAsync();
        Task<UserDTO> LoginAsync(UserDTO person);
        Task<UserDTO> RefreshTokenAsync(UserDTO person);
    }
}