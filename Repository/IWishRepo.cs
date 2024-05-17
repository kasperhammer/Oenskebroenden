using Models.DtoModels;
using Models.Forms;

namespace Repository
{
    public interface IWishRepo
    {
        Task<bool> CreateWishAsync(WishCreateForm wish, UserDTO cookie);
        Task<bool> CreateWishListAsync(WishlistCreateForm wishList, UserDTO cookie);
        Task<WishListDTO> GetOneWishListAsync(UserDTO cookie, int wishListId);
        Task<List<WishListDTO>> GetUseresWishListsAsync(UserDTO cookie);
    }
}