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
    public partial class AuthorPage : ComponentBase
    {

        public Modal WishlistModal { get; set; }
        public Modal WishModal { get; set; }
        public Modal EditWishModal { get; set; }
        public Modal EditWishlistModal { get; set; }

        public WishCreateForm WishToEdit { get; set; } = new();
        public WishlistCreateForm WishlistToEdit { get; set; } = new();

        [Inject]
        IWishRepo WishRepo { get; set; }
        [Inject]
        Auth Auth { get; set; }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                WishToEdit = new()
                {
                    WishListId = 7,
                    Name = "Test",
                    Link = "example.com",
                    Price = 107,
                    Description = "",
                    PictureURL = "chrome://branding/content/about-logo.png"
                };


                WishlistToEdit = new()
                {
                    Name = "Min liste",
                    Emoji = "🌠",
                };

    
                StateHasChanged();
                
            }
        }


       

        private async Task EditWishlistModalOpen()
        {
            EditWishlistModal = new();
            EditWishlistModal.CssDisplay = "block";
            EditWishlistModal.CssShow = "Show";
            EditWishlistModal.ShowModal = true;
            StateHasChanged();
        }

        private async Task WishlistModalOpen()
        {
            WishlistModal = new();
            WishlistModal.CssDisplay = "block";
            WishlistModal.CssShow = "Show";
            WishlistModal.ShowModal = true;
            StateHasChanged();
        }

        private async Task EditWishModalOpen()
        {
            EditWishModal = new();
            EditWishModal.CssDisplay = "block";
            EditWishModal.CssShow = "Show";
            EditWishModal.ShowModal = true;
            StateHasChanged();
        }

        private async Task WishModalOpen()
        {
            WishModal = new();
            WishModal.CssDisplay = "block";
            WishModal.CssShow = "Show";
            WishModal.ShowModal = true;
            StateHasChanged();
        }

        public async Task EditWishlistModalClose()
        {
            EditWishlistModal = new();
            StateHasChanged();
        }
        public async Task WishlistModalClose()
        {
            WishlistModal = new();
            StateHasChanged();
        }

        public async Task EditWishModalClose()
        {
            EditWishModal = new();
            StateHasChanged();
        }
        public async Task WishModalClose()
        {
            WishModal = new();
            StateHasChanged();
        }

        public async Task EditWishListAsync(WishlistCreateForm NewWishlist)
        {
            UserDTO cookie = await Auth.GetUserClaimAsync();
            Console.WriteLine();
        }

        public async Task EditWishAsync(WishCreateForm newWish)
        {
            UserDTO cookie = await Auth.GetUserClaimAsync();
            Console.WriteLine(newWish.Name);
        }

        public async Task DeleteWishAsync()
        {
            UserDTO cookie = await Auth.GetUserClaimAsync();
            Console.WriteLine("DELETED");
        }

        public async Task DeleteWishlistAsync()
        {
            UserDTO cookie = await Auth.GetUserClaimAsync();
            Console.WriteLine("DELETED");
        }

        public async Task CreateWishlist(WishlistCreateForm NewWishlist)
        {
            UserDTO cookie = await Auth.GetUserClaimAsync();
            await WishRepo.CreateWishListAsync(NewWishlist, cookie);
        }

        public async Task CreateWish(WishCreateForm wish)
        {
            UserDTO cookie = await Auth.GetUserClaimAsync();
            await WishRepo.CreateWishAsync(wish, cookie);

        }
    }
}
