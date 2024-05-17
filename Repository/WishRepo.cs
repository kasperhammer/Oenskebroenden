using Models.DtoModels;
using Models.EntityModels;
using Models.Forms;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class WishRepo : IWishRepo
    {
        public WishService wishService;
        public ITokenRepo tokenRepo;

        public WishRepo(ITokenRepo tokenRepo)
        {
            this.tokenRepo = tokenRepo;
            wishService = new();

        }

        /// <summary>
        /// Metode til at oprette en Ønskeliste for en bruger
        /// </summary>
        /// <param name="wishList"></param>
        /// <param name="cookie"></param>
        /// <returns>retunere True hvis ønskelisten blev oprettet</returns>
        public async Task<bool> CreateWishListAsync(WishlistCreateForm wishList, UserDTO cookie)
        {
            cookie = await tokenRepo.TokenValidationPackageAsync(cookie);
            if (wishList != null && cookie != null)
            {
                return await wishService.CreateWishListAsync(wishList, cookie);
            }
            return false;
        }

        // Denne metode henter ønskelister for en bruger.
        public async Task<List<WishListDTO>> GetUseresWishListsAsync(UserDTO cookie)
        {
            // Validerer brugerens token ved at kalde TokenValidationPackage fra tokenRepo.
            cookie = await tokenRepo.TokenValidationPackageAsync(cookie);

            // Tjekker om token valideringen var succesfuld.
            if (cookie != null)
            {
                // Hvis valideringen var succesfuld, hentes ønskelister for brugeren via WishService.
                return await wishService.GetWishlistsFromUserAsync(cookie);
            }

            // Returnerer null hvis token valideringen fejlede eller hvis brugeren ikke har nogen ønskelister.
            return null;
        }


        /// <summary>
        /// Metode til at oprette et ønske for en bruger, på en ønskeliste
        /// </summary>
        /// <param name="wish"></param>
        /// <param name="cookie"></param>
        /// <returns>retunere True hvis ønsket blev oprettet</returns>
        public async Task<bool> CreateWishAsync(WishCreateForm wish, UserDTO cookie)
        {
            cookie = await tokenRepo.TokenValidationPackageAsync(cookie);
            if (wish != null && cookie != null)
            {
                return await wishService.CreateWishAsync(wish, cookie);
            }
            return false;
        }


        public async Task<WishListDTO> GetOneWishListAsync(UserDTO cookie, int wishListId)
        {
            cookie = await tokenRepo.TokenValidationPackageAsync(cookie);
            if (wishListId != 0 && cookie != null)
            {
                return await wishService.GetOneWishListASync(cookie.Token, wishListId);
            }
            return null;
        }

        public async Task<bool> ReserveWishAsync(UserDTO cookie, int wishId)
        {
            cookie = await tokenRepo.TokenValidationPackageAsync(cookie);
            if (wishId != 0 && cookie != null)
            {
                return await wishService.ReserveWishAsync(cookie.Token, wishId);
            }
            return false;
        }

        public async Task<bool> UpdateWishAsync(WishCreateForm wish, UserDTO cookie)
        {
            cookie = await tokenRepo.TokenValidationPackageAsync(cookie);
            if (wish != null && cookie != null)
            {
                return await wishService.UpdateWishASync(wish, cookie.Token);
            }
            return false;
        }

        public async Task<bool> DeleteWishAsync(UserDTO cookie, int wishId)
        {
            cookie = await tokenRepo.TokenValidationPackageAsync(cookie);
            if (wishId != 0 && cookie != null)
            {
                return await wishService.DeleteWishAsync(cookie.Token, wishId);
            }
            return false;
        }
    }
}
