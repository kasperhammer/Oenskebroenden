using Models.DtoModels;
using Models.Forms;

namespace Repo
{
    public interface IHistoryRepo
    {
        Task<bool> AddHistory(HistoryForm historyUI);
        Task<List<HistoryDTO>> GetHistoryAsync(int userId);
    }
}