using Models.DtoModels;
using Models.Forms;
using ServiceLayer;

namespace Repository
{
    public class AccountRepo : IAccountRepo
    {

        AccountService service;
        public ITokenRepo tokenRepo;

        public AccountRepo(ITokenRepo tokenRepo)
        {
            this.tokenRepo = tokenRepo;
            service = new();

        }

        public async Task<UserDTO> LoginAsync(string userName, string password)
        {
            //Kalder videre ned i mit Service Lag mhb på at logge brugeren ind.
            return await service.LoginAsync(userName, password);
        }
        public async Task<UserDTO> CreateAccountAsync(UserCreateForm userForm)
        {
            if (userForm != null)
            {
                try
                {
                    //Kalder videre ned i mit Service Lag mhb på at oprette brugeren.
                    return await service.CreateUserAsync(userForm);
                }
                catch { }
            }
            return null;
        }


        public async Task<bool> TestMetode(string someParam, UserDTO cookie)
        {
            cookie = await tokenRepo.TokenValidationPackageAsync(cookie);
            if (cookie != null)
            {
                //Skriv metode logik her !
            }
            return false;
        }

    }
}
