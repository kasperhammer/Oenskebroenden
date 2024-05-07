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
        public TokenUpdateService tokenUpdateService;
        AccountService service;

        public TokenRepo(TokenUpdateService tokenUpdateService)
        {
            service = new();
            tokenUpdateService = tokenUpdateService;
        }
        public async Task<UserDTO> RefreshTokenAsync(UserDTO person)
        {
            if (person != null)
            {
                return await service.RefreshTokenAsync(person);
            }
            return null;
        }
        public async Task<bool> ValidateTokenAsync(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                return await service.ValidateToken(token);
            }
            return false;
        }

        public bool IsTokenAboutToExpire(DateTime tokenExpiration)
        {
            return service.IsTokenAboutToExpire(tokenExpiration);
        }

        public async Task<UserDTO> TokenValidationPackage(UserDTO cookie)
        {
            if (await ValidateTokenAsync(cookie.Token))
            {
                if (!IsTokenAboutToExpire(cookie.TokenExpires.Value))
                {
                    return cookie;
                }
                else
                {
                    cookie = await RefreshTokenAsync(cookie);
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
