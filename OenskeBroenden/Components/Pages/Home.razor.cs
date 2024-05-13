using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Models.DtoModels;
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

        }
    }
}
