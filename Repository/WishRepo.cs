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
                wishList.OwnerId = cookie.Id;
                return await WishService.CreateWishListAsync(wishList, cookie);
            }
            return false;
        }

        public async Task<List<WishListDTO>> GetUseresWishLists(UserDTO cookie)
        {
            cookie = await tokenRepo.TokenValidationPackage(cookie);
            if (cookie != null)
            {
                return await WishService.GetWishlistsFromUser(cookie);
            }
            return null;
        }

    }
}
