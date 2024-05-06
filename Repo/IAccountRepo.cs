using Models.DtoModels;

namespace Repo
{
    public interface IAccountRepo
    {
        Task<bool> CreateAccountAsync(UserDTO userDto);
    }
}