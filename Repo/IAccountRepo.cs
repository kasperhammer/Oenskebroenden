using Models.DtoModels;
using Models.Forms;

namespace Repo
{
    public interface IAccountRepo
    {
        Task<bool> CreateAccountAsync(UserCreateForm userDto);
    }
}