using DbAccess;
using Microsoft.EntityFrameworkCore;
using Models.DtoModels;
using Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo
{
    public class WishRepo
    {
        EntityContext dBLayer;
        AutoMapper autoMapper;

        public WishRepo()
        {
            dBLayer = new();
            autoMapper = new();
        }

        public async Task<bool> CreateWishAsync(WishDTO wishDTO)
        {
            if (wishDTO != null)
            {
                //Jeg anvender min AutoMapper til at konvertere min DTO model om til en EntityModel
                Wish wish = autoMapper.mapper.Map<Wish>(wishDTO);
                if (wish != null)
                {
                    try
                    {
                        await dBLayer.Wishes.AddAsync(wish);
                        return await dBLayer.SaveChangesAsync() > 0;
                    }
                    catch
                    { }
                }
            }
            return false;
        }


        public async Task<bool> CreateWishlistAsync(WishListDTO wishlistDTO)
        {
            if (wishlistDTO != null)
            {
                //Jeg anvender min AutoMapper til at konvertere min DTO model om til en EntityModel
                WishList wishlist = autoMapper.mapper.Map<WishList>(wishlistDTO);
                wishlist.Owner = null;
                wishlist.Wishes = null;
                wishlist.Chat = new();
                if (wishlist != null)
                {
                    wishlist.OwnerId = wishlistDTO.OwnerId;

                    try
                    {
                        await dBLayer.WishLists.AddAsync(wishlist);
                        return await dBLayer.SaveChangesAsync() > 0;
                    }
                    catch
                    { }
                }
            }
            return false;
        }



        public async Task<List<WishListDTO>> GetWishlistsFromUser(int userId)
        {
            List<WishList> tWishlists = await dBLayer.WishLists.Where(w => w.OwnerId == userId).Include(w => w.Wishes).ToListAsync();
            List<WishListDTO> rWishlists = new();
            foreach (WishList w in tWishlists)
            {
                rWishlists.Add(autoMapper.mapper.Map<WishListDTO>(w));
            }
            return rWishlists;
        }
    }
}
