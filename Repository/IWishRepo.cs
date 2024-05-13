using Models.DtoModels;
using Models.Forms;

namespace Repository
{
    public interface IWishRepo
    {
        Task<bool> CreateWishListAsync(WishlistCreateForm wishList, UserDTO cookie);
        public Task<bool> CreateWishAsync(WishCreateForm wish, UserDTO cookie);
        Task<List<WishListDTO>> GetUseresWishLists(UserDTO cookie);
    }
}