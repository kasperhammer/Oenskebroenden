using DbAccess;
using Microsoft.EntityFrameworkCore;
using Models.DtoModels;
using Models.EntityModels;
using Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo
{
    public class HistoryRepo : IHistoryRepo
    {
        EntityContext dBLayer;
        AutoMapper autoMapper;

        public HistoryRepo()
        {
            dBLayer = new();
            autoMapper = new();
        }

        public async Task<bool> AddHistory(HistoryForm historyUI)
        {
            if (historyUI != null)
            {
                History history = autoMapper.mapper.Map<History>(historyUI);
                if (history != null)
                {
                    history.User = null;
                    history.WishList = null;

                    List<int> histories = await dBLayer.Histories.Where(h => h.UserId == history.UserId).Take(5).Select(h => h.WishListId).ToListAsync();

                    if (histories.First() != history.WishListId)
                    {
                        await dBLayer.Histories.AddAsync(history);
                        return await dBLayer.SaveChangesAsync() > 0; 
                    }

                }


            }
            return false;
        }
    }
}
