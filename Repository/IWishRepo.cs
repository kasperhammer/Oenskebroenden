using Models.DtoModels;
using Models.Forms;

namespace Repository
{
    public interface IWishRepo
    {
        Task<bool> CreateWishAsync(WishCreateForm wish, UserDTO cookie);
        Task<bool> CreateWishListAsync(WishlistCreateForm wishList, UserDTO cookie);
        Task<bool> DeleteWishAsync(UserDTO cookie, int wishId);
        Task<WishListDTO> GetOneWishListAsync(UserDTO cookie, int wishListId);
        Task<List<WishListDTO>> GetUseresWishListsAsync(UserDTO cookie);
        Task<bool> ReserveWishAsync(UserDTO cookie, int wishId);
        Task<bool> UpdateWishAsync(WishCreateForm wish, UserDTO cookie);
    }
}