﻿using DbAccess;
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

        public async Task<bool> AddHistoryAsync(HistoryForm historyUI)
        {
            if (historyUI != null)
            {
                History history = autoMapper.mapper.Map<History>(historyUI);
                if (history != null)
                {
                    history.User = null;
                    history.WishList = null;

                    List<int> histories = await dBLayer.Histories.Where(h => h.UserId == history.UserId).OrderByDescending(x => x.Id).Take(5).Select(h => h.WishListId).ToListAsync();


                    if (histories.Count != 0)
                    {
                        if (histories.First() == history.WishListId)
                        {
                            return false;

                        }
                    }
                    await dBLayer.Histories.AddAsync(history);
                    return await dBLayer.SaveChangesAsync() > 0;



                }


            }
            return false;
        }

        public async Task<List<HistoryDTO>> GetHistoryAsync(int userId)
        {
            if (userId != null)
            {
                List<History> history = await dBLayer.Histories.Include(x => x.WishList)
        .ThenInclude(wishList => wishList.Wishes)
    .Include(x => x.WishList)
        .ThenInclude(wishList => wishList.Owner)
    .Where(x => x.UserId == userId)
    .ToListAsync();

                if (history != null)
                {
                    if (history.Count != 0)
                    {
                        List<HistoryDTO> histories = new();
                        foreach (var item in history)
                        {
                            histories.Add(autoMapper.mapper.Map<HistoryDTO>(item));
                        }
                        histories = histories.OrderByDescending(x => x.Id).ToList();
                        return histories;
                    }
                }
            }
            return null;
        }
    }
}
