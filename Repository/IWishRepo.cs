using Models.DtoModels;
using Models.Forms;

namespace Repository
{
    public interface IWishRepo
    {
        Task<bool> CreateWishListAsync(WishlistCreateForm wishList, UserDTO cookie);
    }
}