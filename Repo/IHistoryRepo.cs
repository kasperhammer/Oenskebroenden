using Models.DtoModels;
using Models.Forms;

namespace Repo
{
    public interface IHistoryRepo
    {
        Task<bool> AddHistoryAsync(HistoryForm historyUI);
        Task<List<HistoryDTO>> GetHistoryAsync(int userId);
    }
}