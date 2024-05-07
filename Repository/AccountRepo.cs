using Models.DtoModels;
using Models.Forms;
using ServiceLayer;

namespace Repository
{
    public class AccountRepo : IAccountRepo
    {
        AccountService service;

        public AccountRepo()
        {
            service = new();
        }

        public async Task<UserDTO> CreateAccountAsync(UserCreateForm userForm)
        {
            if (userForm != null)
            {
                try
                {
                    return await service.CreateUser(userForm);
                }
                catch { }
            }
            return null;
        }
    }
}
