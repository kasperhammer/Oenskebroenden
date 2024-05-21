using ComponentLib;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.JSInterop;
using Models;
using Models.DtoModels;
using Models.EntityModels;
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
        IAccountRepo Repo { get; set; }

        [Inject]
        IWishRepo WishRepo { get; set; }

        [Inject]
        IHistoryRepo HistoryRepo { get; set; }

        [Inject]
        Auth Auth { get; set; }

        [Inject]
        NavigationManager NavMan { get; set; }

        [Inject]
        SignalRUtil SignalR { get; set; }

   

        UserDTO Cookie { get; set; }

        WishListDTO SelectedList { get; set; } = new();

        [Parameter]
        public int WishListId { get; set; }



        bool wishListOwner = false;

        public bool ready;

        bool modal;

        bool addWish;

        bool editWish;

        string color;

        public WishCreateForm WishCreateForm { get; set; } = new();




        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

                // Henter brugerens token fra godkendelsesservice.
                Cookie = await Auth.GetUserClaimAsync();
                Cookie.WishLists = await WishRepo.GetUseresWishListsAsync(Cookie);
           
                // Opretter en ny ønskeliste, hvis brugeren ikke har nogen.
                if (Cookie.WishLists == null)
                {
                    Cookie.WishLists = new();
                }

                if (WishListId != 0)
                {
                    if (Cookie.WishLists.FirstOrDefault(x => x.Id == WishListId) == null)
                    {
                        //This WishList is not one of our Own
                        wishListOwner = false;
                        SelectedList = await WishRepo.GetOneWishListAsync(Cookie, WishListId);
                        //Add To history
                        await HistoryRepo.AddHistoryAsync(Cookie, WishListId);
                    }
                    else
                    {
                        wishListOwner = true;
                        await LoadWishlistAsync(WishListId);
                        //this is one of my own wishlists

                    }
                }

                Cookie.WishListHistory = await HistoryRepo.GetHistoryDTOsAsync(Cookie);
                // Kalder en testmetode på konto repository med brugerens cookie.
                await Repo.TestMetode("SomeParam", Cookie);

                // Indikerer, at komponenten er klar til visning.
                ready = true;

                // Opdaterer komponentens tilstand.
                StateHasChanged();
            }
        }

     

        public async Task LoadWishlistAsync(int wishlistId)
        {
            WishListDTO tempList = Cookie.WishLists.FirstOrDefault(x => x.Id == wishlistId);
            if (tempList != null)
            {
                SelectedList = tempList;
                wishListOwner = true;
            }
            else
            {
                SelectedList = await WishRepo.GetOneWishListAsync(Cookie, wishlistId);
                await HistoryRepo.AddHistoryAsync(Cookie, wishlistId);
                Cookie.WishListHistory = await HistoryRepo.GetHistoryDTOsAsync(Cookie);
                wishListOwner = false;
            }

            SignalR.ConnectAsync(Cookie, SelectedList.Id);
            StateHasChanged();
        }

        // Metode til at vise eller skjule oprettelse af en ønskeliste modal.
        public async Task ToggleCreateList(bool success)
        {
            if (success)
            {
                await UpdateCookie();
            }
            modal = !modal;
            StateHasChanged();
        }

        public async Task ToggleAddWish(bool success)
        {
            if (success)
            {
                await UpdateCookie();
                addWish = false;
            }
            else
            {
                addWish = true;
            }

            StateHasChanged();
        }

        public async Task ToggleEditWish(WishCreateForm? w)
        {
            if (w == null)
            {
                await UpdateCookie();
            }
            else
            {
                WishCreateForm = w;
            }
            editWish = !editWish;
            StateHasChanged();
        }


        public void HomeButton()
        {
            SelectedList = new();
            WishListId = 0;
            wishListOwner = false;
            StateHasChanged();
        }



        public async Task UpdateCookie()
        {
            Cookie.WishLists = await WishRepo.GetUseresWishListsAsync(Cookie);
            Cookie.WishListHistory = await HistoryRepo.GetHistoryDTOsAsync(Cookie);
            if (SelectedList.Id != 0)
            {
                int id = SelectedList.Id;
                SelectedList = Cookie.WishLists.FirstOrDefault(x => x.Id == SelectedList.Id);
                if (SelectedList == null)
                {
                    SelectedList = await WishRepo.GetOneWishListAsync(Cookie, id);
                }
            }

            StateHasChanged();
        }

       public async Task SendMessageAsync(ChatMessageForm message)
        {
            await SignalR.SendMessage(message);
        }

    }
}


