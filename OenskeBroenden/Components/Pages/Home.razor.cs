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
                // Henter brugerens token fra godkendelsesservice.
                cookie = await Auth.GetUserClaimAsync();



                cookie.WishLists = await wishrepo.GetUseresWishLists(cookie);


                // Opretter en ny ønskeliste, hvis brugeren ikke har nogen.
                if (cookie.WishLists == null)
                {
                    cookie.WishLists = new();
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
    }
}


