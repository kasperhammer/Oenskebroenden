using Models.DtoModels;
using Models.Forms;

namespace Repository
{
    public interface IAccountRepo
    {
        Task<UserDTO> CreateAccountAsync(UserCreateForm userForm);
        Task<UserDTO> LoginAsync(string userName, string password);
        Task<bool> TestMetode(string someParam, UserDTO cookie);
    }
}