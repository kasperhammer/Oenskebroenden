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
    public class WishRepo
    {
        EntityContext dBLayer;
        AutoMapper autoMapper;

        public WishRepo()
        {
            dBLayer = new();
            autoMapper = new();
        }

        /// <summary>
        /// Metode til at oprette et ønske for en bruger
        /// </summary>
        /// <param name="wishDTO"></param>
        /// <returns>Retunere True hvis ønskelisten blev oprettet</returns>
        public async Task<bool> CreateWishAsync(WishDTO wishDTO)
        {
            if (wishDTO != null)
            {
                //Jeg anvender min AutoMapper til at konvertere min DTO model om til en EntityModel
                Wish wish = autoMapper.mapper.Map<Wish>(wishDTO);
                wish.ReservedUser = null;
                wish.ReservedUserId = null;
                wish.WishList = null;
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

        /// <summary>
        /// Metode til at oprette en ønskeliste for en bruger
        /// </summary>
        /// <param name="wishlistDto"></param>
        /// <returns>Retunere True hvis ønskelisten blev oprettet</returns>
        public async Task<bool> CreateWishlistAsync(WishListDTO wishlistDto)
        {
            if (wishlistDto != null)
            {
                //Jeg anvender min AutoMapper til at konvertere min DTO model om til en EntityModel
                WishList wishlist = autoMapper.mapper.Map<WishList>(wishlistDto);
                wishlist.Owner = null;
                wishlist.Wishes = null;
                wishlist.Chat = new();
                if (wishlist != null)
                {
                    wishlist.OwnerId = wishlistDto.OwnerId;

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


        /// <summary>
        /// Metode til at Hente en brugeres ønskeliste
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Retunere en liste af brugerens ønskelister</returns>
        public async Task<List<WishListDTO>> GetWishlistsFromUserAsync(int userId)
        {
            List<WishList> tWishlists = await dBLayer.WishLists.Where(w => w.OwnerId == userId).Include(w => w.Wishes).ToListAsync();
            List<WishListDTO> rWishlists = new();
            foreach (WishList w in tWishlists)
            {
                rWishlists.Add(autoMapper.mapper.Map<WishListDTO>(w));
            }
            return rWishlists;
        }

        public async Task<bool> DeleteWishListAsync(int wishlistId)
        {
            if (wishlistId != 0)
            {
                WishList wishlist = await dBLayer.WishLists.Include(x => x.Wishes).FirstOrDefaultAsync(x => x.Id == wishlistId);
                try
                {
                    if (wishlist != null)
                    {
                        dBLayer.WishLists.Remove(wishlist);
                        return await dBLayer.SaveChangesAsync() > 0;
                    }
                }
                catch { }
            }
            return false;
        }

        /// <summary>
        /// Metode til at slette et ønske
        /// </summary>
        /// <param name="wishId"></param>
        /// <returns>Retunere om et ønske blev slettet</returns>
        public async Task<bool> DeleteWishAsync(int wishId)
        {
            if (wishId != 0)
            {
                Wish wish = await dBLayer.Wishes.FirstOrDefaultAsync(x => x.Id == wishId);
                if (wish != null)
                {
                    try
                    {
                        dBLayer.Wishes.Remove(wish);
                        return await dBLayer.SaveChangesAsync() > 0;
                    }
                    catch { }
                }
            }
            return false;
        }

        public async Task<bool> EditWishAsync(WishCreateForm wishCreate)
        {
            Wish wish = autoMapper.mapper.Map<Wish>(wishCreate);
            dBLayer.Wishes.Update(wish);
            return await dBLayer.SaveChangesAsync() > 0;
          
        }


        /// <summary>
        /// Metode til at resevere et ønske
        /// </summary>
        /// <param name="wishId"></param>
        /// <param name="userId"></param>
        /// <returns>Retunere True hvis ønsket blev reseveret</returns>
        public async Task<bool> ReserveWishAsync(int wishId, int userId)
        {
            if (wishId != 0)
            {
                Wish wish = await dBLayer.Wishes.FirstOrDefaultAsync(wish => wish.Id == wishId);

                if (wish != null)
                {
                    try
                    {

                        if (wish.ReservedUserId == null)
                        {
                            wish.ReservedUserId = userId;
                        }
                        else
                        {
                            if (wish.ReservedUserId == userId)
                            {
                                wish.ReservedUserId = null;
                            }
                        }
                        wish.ReservedUser = null;
                        dBLayer.Wishes.Update(wish);
                        //    int rows = await dBLayer.Wishes.Where(w=> w.Id == wishId).ExecuteUpdateAsync(x => x.SetProperty(r => r.ReservedUserId, r =>  userId));
                        return await dBLayer.SaveChangesAsync() > 0;

                    }
                    catch { }
                }
            }
            return false;
        }

        public async Task<WishListDTO> GetOneWishListAsync(int wishlistId)
        {
            if (wishlistId != 0)
            {
                WishList wishList = await dBLayer.WishLists.Include(x => x.Wishes).Include(x => x.Chat).FirstOrDefaultAsync(x => x.Id == wishlistId);
                if (wishList != null)
                {
                    WishListDTO wishListDTO = autoMapper.mapper.Map<WishListDTO>(wishList);
                    if (wishListDTO != null)
                    {
                        return wishListDTO;
                    }
                }
            }
            return null;
        }

    }
}
