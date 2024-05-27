using Models.DtoModels;
using Models.Forms;

namespace Repo
{
    public interface IWishRepo
    {
        Task<bool> CreateWishAsync(WishDTO wishDTO);
        Task<bool> CreateWishlistAsync(WishListDTO wishlistDto);
        Task<bool> DeleteWishAsync(int wishId);
        Task<bool> DeleteWishListAsync(int wishlistId);
        Task<bool> EditWishAsync(WishCreateForm wishCreate);
        Task<WishListDTO> GetOneWishListAsync(int wishlistId);
        Task<List<WishListDTO>> GetWishlistsFromUserAsync(int userId);
        Task<bool> ReserveWishAsync(int wishId, int userId);
    }
}