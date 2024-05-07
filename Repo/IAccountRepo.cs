using Models.DtoModels;
using Models.Forms;

namespace Repo
{
    public interface IAccountRepo
    {
        Task<UserDTO> CreateAccountAsync(UserCreateForm userDto);
    }
}