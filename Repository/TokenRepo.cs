using Models.DtoModels;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class TokenRepo : ITokenRepo
    {
        private static TokenUpdateService tokenUpdateService;
        AccountService service;

        public TokenRepo(TokenUpdateService _tokenUpdateService)
        {
            service = new();
            if (tokenUpdateService == null)
            {
                tokenUpdateService = _tokenUpdateService; 
            }
        }

        //Denne motde kalder min Api og henter en Frisk Token, såfremt burgerns
        //nuværene token stadig er gyldig.
        public async Task<UserDTO> RefreshTokenAsync(UserDTO person)
        {
            if (person != null)
            {
                return await service.RefreshTokenAsync(person);
            }
            return null;
        }

        //Denne metoder kalder end mod et Authenticatet Endpoint så såfremt ens token ikke er gyldig får du false
        //såfremt den stadig er gyldig for du true
        public async Task<bool> ValidateTokenAsync(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                return await service.ValidateToken(token);
            }
            return false;
        }

        //Denne metode fortæller dig om din token er inde for en halv time af at udløbe
        public bool IsTokenAboutToExpire(DateTime tokenExpiration)
        {
            return service.IsTokenAboutToExpire(tokenExpiration);
        }

        //Samlet pakke som kan bruges af de andre repos i toppen af deres metoder til at kontrollere at 
        //brugerns token stadig er valid og såfremt der er brug for det også få udstet en frisk token
        public async Task<UserDTO> TokenValidationPackage(UserDTO cookie)
        {
            //Først tjekker jeg om min token stadig er valid
            if (await ValidateTokenAsync(cookie.Token))
            {
                //her tjekker jeg om min token er inde for 30 minutter af at udløbe
                if (!IsTokenAboutToExpire(cookie.TokenExpires.Value))
                {
                    return cookie;
                }
                else
                {
                    //såfremt den er henter jeg en ny token
                    cookie = await RefreshTokenAsync(cookie);
                    //derefter Pusher jeg den nye cookie ud til de subscibere der lytter efter eventuelle nye tokens
                    //fx Auth.
                    tokenUpdateService.RaiseEvent(cookie);
                    //renew Token process
                    return cookie;
                }
            }
            cookie = null;
            tokenUpdateService.RaiseEvent(cookie);
            return cookie;
        }
    }
}
