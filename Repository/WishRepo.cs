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
        WishService WishService { get; set; }
        public ITokenRepo tokenRepo;

        public WishRepo(ITokenRepo tokenRepo)
        {
            this.tokenRepo = tokenRepo;
            WishService = new();

        }

        public async Task<bool> CreateWishListAsync(WishlistCreateForm wishList, UserDTO cookie)
        {
            cookie = await tokenRepo.TokenValidationPackage(cookie);
            if (wishList != null && cookie != null)
            {
                return await WishService.CreateWishListAsync(wishList, cookie);
            }
            return false;
        }

        // Denne metode henter ønskelister for en bruger.
        public async Task<List<WishListDTO>> GetUseresWishLists(UserDTO cookie)
        {
            // Validerer brugerens token ved at kalde TokenValidationPackage fra tokenRepo.
            cookie = await tokenRepo.TokenValidationPackage(cookie);

            // Tjekker om token valideringen var succesfuld.
            if (cookie != null)
            {
                // Hvis valideringen var succesfuld, hentes ønskelister for brugeren via WishService.
                return await WishService.GetWishlistsFromUser(cookie);
            }

            // Returnerer null hvis token valideringen fejlede eller hvis brugeren ikke har nogen ønskelister.
            return null;
        }


        public async Task<bool> CreateWishAsync(WishCreateForm wish, UserDTO cookie)
        {
            cookie = await tokenRepo.TokenValidationPackage(cookie);
            if (wish != null && cookie != null)
            {
                return await WishService.CreateWishAsync(wish, cookie);
            }
            return false;
        }

    }
}
