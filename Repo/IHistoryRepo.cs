using Models.Forms;

namespace Repo
{
    public interface IHistoryRepo
    {
        Task<bool> AddHistory(HistoryForm historyUI);
    }
}