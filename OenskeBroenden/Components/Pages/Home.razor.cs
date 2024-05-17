using ComponentLib;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
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


        UserDTO Cookie { get; set; }

        WishListDTO SelectedList { get; set; } = new();

        [Parameter]
        public int WishListId { get; set; }

        [Inject]
        NavigationManager NavMan { get; set; }

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
                Cookie.WishListHistory = await HistoryRepo.GetHistoryDTOsAsync(Cookie);


                // Opretter en ny ønskeliste, hvis brugeren ikke har nogen.
                if (Cookie.WishLists == null)
                {
                    Cookie.WishLists = new();
                }

                if(WishListId != 0)
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
            SelectedList = Cookie.WishLists.FirstOrDefault(x => x.Id == wishlistId);
            wishListOwner = true;
            StateHasChanged();
        }

        // Metode til at vise eller skjule oprettelse af en ønskeliste modal.
        public async Task ShowCreateListModal(bool show)
        {
            modal = show;
            StateHasChanged();
        }

        // Metode til at oprette en ønskeliste.
        public async Task CreateWishlist(WishlistCreateForm NewWishlist)
        {
            // Opretter en ny ønskeliste ved hjælp af WishRepo med den nye ønskeliste og brugerens cookie.
            await WishRepo.CreateWishListAsync(NewWishlist, Cookie);

            // Opdaterer brugerens ønskelister i brugerens cookie.
            Cookie.WishLists = await WishRepo.GetUseresWishListsAsync(Cookie);

            // Skjuler modalen efter oprettelsen af ønskelisten.
            await ShowCreateListModal(false);
        }

        public async Task CreateWish(WishCreateForm newWish)
        {
     
            if (await WishRepo.CreateWishAsync(newWish, Cookie))
            {
                Cookie.WishLists = await WishRepo.GetUseresWishListsAsync(Cookie);
                SelectedList = Cookie.WishLists.FirstOrDefault(x => x.Id == SelectedList.Id);
                await ToggleAddWish(); 
            }
        }

        public async Task ToggleAddWish()
        {
            addWish = !addWish;
            StateHasChanged();
        }

        public void HomeButton()
        {
            SelectedList = new();
            WishListId = 0;
            wishListOwner = false;
            StateHasChanged();
        }

        public async Task ToggleEditWishAsync(WishDTO? w)
        {
            if (w != null)
            {
                WishCreateForm = new WishCreateForm { Id = w.Id,Description = w.Description,Link = w.Link,Name = w.Name,PictureURL = w.PictureURL,Price = w.Price,WishListId = w.WishListId};
            }
            editWish = !editWish;
            StateHasChanged();
        }


    }
}


