using ComponentLib;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Models;
using Models.DtoModels;
using Models.Forms;
using OenskeBroenden.Utils;
using Repository;
using System;

namespace OenskeBroenden.Components.Pages
{
    [Authorize]
    public partial class Home
    {

        [Inject]
        IAccountRepo repo { get; set; }

        [Inject]
        IWishRepo wishrepo { get; set; }

        [Inject]
        Auth Auth { get; set; }
        UserDTO cookie { get; set; }
    
        public bool ready;
        bool modal;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

                cookie = await Auth.GetUserClaimAsync();

                cookie.WishLists = await wishrepo.GetUseresWishLists(cookie);
                if (cookie.WishLists == null)
                {
                    cookie.WishLists = new();
                }

                await repo.TestMetode("SomeParam", cookie);
                ready = true;
                StateHasChanged();
            }
        }

        public async Task LoadWishlistAsync(int wishlistId)
        {

        }

        public async Task ShowCreateListModal(bool show)
        {
            modal = show;
            StateHasChanged();
        }

        public async Task CreateWishlist(WishlistCreateForm NewWishlist)
        {
            await wishrepo.CreateWishListAsync(NewWishlist, cookie);
            cookie.WishLists = await wishrepo.GetUseresWishLists(cookie);
            await ShowCreateListModal(false);
        }


        
    }
}
