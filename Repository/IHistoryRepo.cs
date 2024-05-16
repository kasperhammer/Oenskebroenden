using Models.DtoModels;

namespace Repository
{
    public interface IHistoryRepo
    {
        Task<bool> AddHistoryAsync(UserDTO cookie, int wishListId);
        Task<List<HistoryDTO>> GetHistoryDTOsAsync(UserDTO cookie);
    }
}