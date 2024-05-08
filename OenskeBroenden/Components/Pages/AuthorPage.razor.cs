using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Protocol;
using Models;
using Models.DtoModels;
using Models.EntityModels;
using Models.Forms;
using OenskeBroenden.Utils;
using Repository;

namespace OenskeBroenden.Components.Pages
{
    [Authorize]
    public partial class AuthorPage
    {

        public Modal WishlistModal { get; set; }
        public Modal WishModal { get; set; }


        [Inject]
        IWishRepo WishRepo { get; set; }
        [Inject]
        Auth Auth { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                StateHasChanged();
            }
        }


       

        private async Task WishlistModalOpen()
        {
            WishlistModal.CssDisplay = "block";
            WishlistModal.CsslShow = "Show";
            WishlistModal.ShowModal = true;
            StateHasChanged();
        }
        private async Task WishModalOpen()
        {
            WishModal.CssDisplay = "block";
            WishModal.CsslShow = "Show";
            WishModal.ShowModal = true;
            StateHasChanged();
        }


        public async Task WishlistModalClose()
        {
            WishlistModal = new();
            StateHasChanged();
        }
        public async Task WishModalClose()
        {
            WishModal = new();
            StateHasChanged();
        }


        public async Task CreateWishlist(WishlistCreateForm NewWishlist)
        {
            UserDTO cookie = await Auth.GetUserClaimAsync();
            await WishRepo.CreateWishListAsync(NewWishlist, cookie);
        }

        public async Task CreateWish(WishCreateForm wish)
        {
            Console.WriteLine(wish.Name);
        }
    }
}
