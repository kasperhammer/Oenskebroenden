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
        IAccountRepo repo { get; set; }


        [Inject]
        IWishRepo wishrepo { get; set; }

        [Inject]
        IHistoryRepo historyRepo { get; set; }


        [Inject]
        Auth Auth { get; set; }


        UserDTO cookie { get; set; }

        WishListDTO selectedList { get; set; } = new();

        [Parameter]
        public int WishListId { get; set; }

        [Inject]
        NavigationManager navMan { get; set; }

        bool wishListOwner = false;


        public bool ready;

        bool modal;

        bool addWish;

        string color;


      

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                // Henter brugerens token fra godkendelsesservice.
                cookie = await Auth.GetUserClaimAsync();



                cookie.WishLists = await wishrepo.GetUseresWishLists(cookie);
                cookie.WishListHistory = await historyRepo.GetHistoryDTOsAsync(cookie);


                // Opretter en ny ønskeliste, hvis brugeren ikke har nogen.
                if (cookie.WishLists == null)
                {
                    cookie.WishLists = new();
                }

                if(WishListId != 0)
                {
                    if (cookie.WishLists.FirstOrDefault(x => x.Id == WishListId) == null)
                    {
                        //This WishList is not one of our Own
                        wishListOwner = false;
                        selectedList = await wishrepo.GetOneWishListAsync(cookie, WishListId);
                        //Add To history
                        await historyRepo.AddHistoryAsync(cookie, WishListId);
                    }
                    else
                    {
                        wishListOwner = true;
                        await LoadWishlistAsync(WishListId);
                        //this is one of my own wishlists
               
                    }
                }
                // Kalder en testmetode på konto repository med brugerens cookie.
                await repo.TestMetode("SomeParam", cookie);

                // Indikerer, at komponenten er klar til visning.
                ready = true;

                // Opdaterer komponentens tilstand.
                StateHasChanged();
            }
        }



        public async Task LoadWishlistAsync(int wishlistId)
        {
            selectedList = cookie.WishLists.FirstOrDefault(x => x.Id == wishlistId);
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
            await wishrepo.CreateWishListAsync(NewWishlist, cookie);

            // Opdaterer brugerens ønskelister i brugerens cookie.
            cookie.WishLists = await wishrepo.GetUseresWishLists(cookie);

            // Skjuler modalen efter oprettelsen af ønskelisten.
            await ShowCreateListModal(false);
        }

        public async Task CreateWish(WishCreateForm newWish)
        {
     
            if (await wishrepo.CreateWishAsync(newWish, cookie))
            {
                cookie.WishLists = await wishrepo.GetUseresWishLists(cookie);
                selectedList = cookie.WishLists.FirstOrDefault(x => x.Id == selectedList.Id);
                await ToggleAddWish(); 
            }
        }

        public async Task ToggleAddWish()
        {
            addWish = !addWish;
            StateHasChanged();
        }
    }
}


