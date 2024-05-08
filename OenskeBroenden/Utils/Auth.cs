using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;
using Repository;
using System.Security.Claims;
using Models.DtoModels;

namespace OenskeBroenden.Utils
{
    public class AuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        public Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        {
            return next(context);
        }
    }
    public class Auth : AuthenticationStateProvider
    {
        private readonly ProtectedLocalStorage protectedLocalStorage;

        private readonly IAccountRepo repo;

        private readonly ITokenRepo tokenRepo;

        private readonly ITokenUpdateService tokenUpdateService;

        //jeg for med i min constructer 4 services. 1. protectedLocalStorage til at store og loade min Session Cookie.
        // 2. IAccount repo, dette repo indeholder min Login Metode.
        // 3. tokenUpdateService TokenUpdate Service indeholder en eventhandler jeg kan subscribe til.
        // så når der kommer en frisk Jwt token ind bliver min Auth.cs notificieret og ved at den skal opdatere min Cookie
        // 4. tokenRepo, den indeholder min Validate token metode samt metoden for at refreshe eventuelle tokens.
        public Auth(ProtectedLocalStorage protectedLocalStorage, IAccountRepo repo, ITokenUpdateService tokenUpdateService, ITokenRepo tokenRepo)
        {
            this.repo = repo;
            this.protectedLocalStorage = protectedLocalStorage;
            this.tokenUpdateService = tokenUpdateService;
            this.tokenRepo = tokenRepo;

            //Subscriber til min TokenUpdate Service.
            tokenUpdateService.tokenUpdated += UpdateToken;
        }

        //Get AuthenticationStateAsync er en default metode i AuthenticationStateProvider som jeg så overrider med min egen
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {

            var principal = new ClaimsPrincipal();


            try
            {

                //Jeg henter den seneste Cookie fra localstorrage         
                var storedpricipal = await protectedLocalStorage.GetAsync<string>("identity");
                if (storedpricipal.Success)
                {
                    //derefter converter jeg cookien over til min model
                    UserDTO user = JsonConvert.DeserializeObject<UserDTO>(storedpricipal.Value);
                    if (user != null)
                    {
                        //jeg tjekker om cookien er udløbet
                        if (user.TokenExpires.Value >= DateTime.UtcNow)
                        {
                            //Såfremt den ikke er udløbet tjekker jeg om token stadig er valid med dens signing key
                            if (await tokenRepo.ValidateTokenAsync(user.Token))
                            {


                                //Her tjekker jeg om token er ved at udløbe så jeg kan få en ny så jeg ikke skal logge ind igen
                                if (tokenRepo.IsTokenAboutToExpire(user.TokenExpires.Value))
                                {
                                    //såfremt token er ved at udløber henter jeg en ny
                                    UserDTO newPerson = await tokenRepo.RefreshTokenAsync(user);
                                    if (newPerson != null)
                                    {
                                        //derefter overvrider jeg den seneste cookie fra min localstorage med den nye. som indeholder min nye token
                                        await protectedLocalStorage.SetAsync("identity", JsonConvert.SerializeObject(newPerson));
                                        var identity = CreateIdentityFromUser(newPerson);
                                        principal = new(identity);

                                        return new AuthenticationState(principal);
                                    }
                                }
                                else
                                {
                                    //Såfremt at min token er valid og den ikke er ved at udløbe laver jeg et claim med den existerende identity
                                    var identity = CreateIdentityFromUser(user);
                                    principal = new(identity);
                                    //derefter tager jeg den identity og ligger op i min pricipal
                                    return new AuthenticationState(principal);
                                }
                            }
                        }
                    }
                }
            }
            catch { }





            return new AuthenticationState(principal);
        }



        public async Task<bool> LoginAsync(UserDTO person)
        {
            if (person != null)
            {
                if (!string.IsNullOrEmpty(person.Name) && !string.IsNullOrEmpty(person.Password))
                {
                    //Såfremt Login var en Success retunere den et UserObject med min Token i.
                    UserDTO user = await repo.LoginAsync(person.Name, person.Password);
                    if (user != null)
                    {
                        try
                        {

                            //Jeg sætter/overskriver den sessioncookie der ligger i med identity med min nyeligt hentet Token.
                            await protectedLocalStorage.SetAsync("identity", JsonConvert.SerializeObject(user));

                            var identity = CreateIdentityFromUser(user);
                            //Derefter bliver mit authentication State Opdateret 
                            //hvilket kalder min GetAuthenticationStateAsync Metode
                            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));

                            return true;
                        }
                        catch
                        {


                        }
                    }
                }
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
            return false;

        }

        public async Task<bool> LogoutAsync()
        {
            //Såfremt jeg ønnsker at logge ud cleare jeg bare min Cookie.
            await protectedLocalStorage.DeleteAsync("identity");

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
            return true;
        }

        private static ClaimsIdentity CreateIdentityFromUser(UserDTO user)
        {
            //min cookie som blvier Created indeholder alt mit vigite UserInformation
            return new ClaimsIdentity(new Claim[]
            {
            new (ClaimTypes.Name, user.Name),
            new (ClaimTypes.Hash, user.Password),
            new (ClaimTypes.Email, user.Email),
            new (type:"TokenExpires",value: user.TokenExpires.ToString()),
            new (type:"Token",value: user.Token),
            new (type:"Id",value: user.Id.ToString()),

            }, "IdentityClaim");
        }

        //en hurtig metode der henter mit UserOBject fra min Cookie, istedet for at kontakte serveren og databasen.
        //user objectet bliver naturligtvis ikke dynamisk opdatet som objektet på databasen, men er godt til data fra useren
        //som ikke ændre sig tit.
        public async Task<UserDTO> GetUserClaimAsync()
        {
            try
            {
                var storedPrincipal = await protectedLocalStorage.GetAsync<string>("identity");
                return JsonConvert.DeserializeObject<UserDTO>(storedPrincipal.Value);
            }
            catch { }
            return null;
        }

        //min UpdateToken Metode blvier kaldt såfremt der er sket en Token Update
        //når der er sket en token Update blvier det nye UserDTO objekt pushede ud til de forskellige subscribers
        //og sent med i parameteren TokenEventServiceArgs hvor den ligger inde.
        //Den gamle Cookie bliver så overskrevet med den nye.
        public async void UpdateToken(object source, TokenEventServiceArgs e)
        {
            try
            {
                if (e.cookie != null)
                {
                    await protectedLocalStorage.SetAsync("identity", JsonConvert.SerializeObject(e.cookie));
                }
                else
                {
                    await LogoutAsync();
                }
            }
            catch { }
        }






    }
}
