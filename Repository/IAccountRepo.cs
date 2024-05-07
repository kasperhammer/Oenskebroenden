using Models.DtoModels;
using Models.Forms;

namespace Repository
{
    public interface IAccountRepo
    {
        Task<UserDTO> CreateAccountAsync(UserCreateForm userForm);
    }
}