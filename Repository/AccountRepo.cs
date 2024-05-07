using Models.DtoModels;
using Models.Forms;
using ServiceLayer;

namespace Repository
{
    public class AccountRepo : IAccountRepo
    {

        AccountService service;
        private readonly TokenRepo tokenRepo;

        public AccountRepo(TokenRepo tokenRepo)
        {
            this.tokenRepo = tokenRepo;
            service = new();

        }
        public async Task<UserDTO> LoginAsync(string userName, string password)
        {
            return await service.LoginAsync(userName, password);
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

        public async Task<bool> TestMetode(string someParam, UserDTO cookie)
        {
            cookie = await tokenRepo.TokenValidationPackage(cookie);
            if (cookie != null)
            {
                //Skriv metode logik her !
            }
            return false;
        }

    }
}
